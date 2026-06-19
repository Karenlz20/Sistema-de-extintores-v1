using System.Text.Json.Serialization;

namespace SistemaExtintores.Modelos
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(ClientePersona))]
    [JsonDerivedType(typeof(ClienteEmpresa))]
    public abstract class Cliente
    {
        public int Id { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; } = true;

        public void Activar() { Activo = true; }
        public void Desactivar() { Activo = false; }

        public abstract string ObtenerNombreCompleto();
        public abstract string ObtenerDocumento();
    }
}