using CentroEventos.Aplicaciones.Excepciones;
using CentroEventos.Aplicaciones.Validaciones;

public class ModificarPersonaUseCase
{

    private readonly IRepositorioPersona _repoPersona;

    private readonly IServicioAutorizacion _autorizador;

    public ModificarPersonaUseCase(IRepositorioPersona repoPersona, IServicioAutorizacion autorizador)
    {

        _repoPersona = repoPersona;
        _autorizador = autorizador; //en cuales UseCase hace falta?? los puse en todos por las dudas, veremos en cuales se sacan
    }

    public void Ejecutar(Persona persona, int IdUsuario)
    {
        try
        {
            string mensajeError;
            ValidarPersona validador = new ValidarPersona(_repoPersona);
            if (!_autorizador.PoseeElPermiso(IdUsuario, Permiso.UsuarioModificacion))
            {
                throw new FalloAutorizacionException();
            }
            
            if (!validador.CamposVacios(persona.Nombre, persona.Apellido, persona.Dni,persona.Email,out mensajeError))
            {
                throw new ValidacionException(mensajeError);
            }
            
            if (!validador.DNINoSeRepite(persona.Dni, out mensajeError))
            {
                throw new DuplicadoException(mensajeError);
            }
            
            if (!validador.EmailNoSeRepite(persona.Email,out mensajeError))
            {
                throw new DuplicadoException(mensajeError);
            }

        }
        catch (FalloAutorizacionException e)
        {
            Console.WriteLine($"Hubo un error de autorizacion: {e.Message}");
        }
        catch (ValidacionException e)
        {
            Console.WriteLine($"Hubo un error de validacion: {e.Message}");
        }
        catch (DuplicadoException e)
        {
            Console.WriteLine($"Hubo un error de duplicado: {e.Message}");
        }
      
        // Revisar este último try/catch
        try
        {
            _repoPersona.ModificarPersona(persona);
        }
        catch (Exception e)
        {
            Console.WriteLine($"No se pudo modificar los datos del usuario: {e.Message}");
        }
    }
} 