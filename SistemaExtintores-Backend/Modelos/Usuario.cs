namespace SistemaExtintores.Modelos;

public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; }
    public string NombreCompleto { get; set; }
    public int? ClienteId { get; set; }
    public string Password { get; set; }
    public RolUsuario Rol { get; set; }
    public bool Activo { get; set; } = true;

    public Usuario() { }

    public Usuario(int id, string nombreUsuario, string password, RolUsuario rol, string nombreCompleto, int? clienteId)
    {
        Id = id;
        NombreUsuario = nombreUsuario;
        Password = password;
        Rol = rol;
        NombreCompleto = nombreCompleto;
        ClienteId = clienteId;
    }

    public void Activar() { Activo = true; }
    public void Desactivar() { Activo = false; }
}