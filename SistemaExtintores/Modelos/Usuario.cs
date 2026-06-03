namespace SistemaExtintores.Modelos;

// En el futuro se añadirán las clases de gestión de empleados, en ese caso, el idUsuario iría como atributo de las clases
// "Cliente" y "Empleado". Por mientras, se pondrá el nombre del empleado en el usuario
public class Usuario
{
    private int _id;
    private string _nombreUsuario;
    private string? _nombreCompleto; // null si el user es cliente, tiene valor si es empleado
    private int? _clienteId; // null si es empleado, tiene valor si la cuenta es de un cliente
    private string _password;
    private RolUsuario _rol;
    private bool _activo = true;

    public Usuario() { }

    public Usuario(int id, string nombreUsuario, string password, RolUsuario rol, string? nombreCompleto, int? clienteId)
    {
        SetId(id);
        SetNombreUsuario(nombreUsuario);
        SetPassword(password);
        SetRol(rol);
        SetNombreCompleto(nombreCompleto);
        SetClienteId(clienteId);
    }

    public int GetId() { return _id; }
    public void SetId(int id) { _id = id; }

    public string GetNombreUsuario() { return _nombreUsuario; }
    public void SetNombreUsuario(string nombreUsuario) { _nombreUsuario = nombreUsuario; }

    public string? GetNombreCompleto() { return _nombreCompleto; }
    public void SetNombreCompleto(string? nombreCompleto) { _nombreCompleto = nombreCompleto; }

    public int? GetClienteId() { return _clienteId; }
    public void SetClienteId(int? clienteId) { _clienteId = clienteId; }

    public string GetPassword() { return _password; }
    public void SetPassword(string password) { _password = password; }

    public RolUsuario GetRol() { return _rol; }
    public void SetRol(RolUsuario rol) { _rol = rol; }

    public bool GetActivo() { return _activo; }
    public void Activar() { _activo = true; }
    public void Desactivar() { _activo = false; }
}