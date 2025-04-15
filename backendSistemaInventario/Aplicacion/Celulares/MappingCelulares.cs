using AutoMapper;
using backendSistemaInventario.DTOS;
using backendSistemaInventario.Modelo;

namespace backendSistemaInventario.Aplicacion.Celulares
{
    public class MappingCelulares : Profile
    {
        public MappingCelulares()
        {
            CreateMap<Celular, CelularDTO>();
        }
    }
}
