using backendSistemaInventario.DTOS;

namespace backendSistemaInventario.Modelo
{
    public class CelularDTO
    {
 
        public int idCelular { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public string compania { get; set; }
        public string numSerie { get; set; }
        public string imei { get; set; }
        public string numCelular { get; set; }
        public int idUsuario { get; set; }
        public UsuariosDTO usuario { get; set; }
    }
}
