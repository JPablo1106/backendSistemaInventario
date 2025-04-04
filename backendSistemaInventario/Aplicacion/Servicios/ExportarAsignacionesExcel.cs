using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using backendSistemaInventario.DTOS;

namespace backendSistemaInventario.Aplicacion.Servicios
{
    public class ExportarAsignacionesExcel
    {
        public static async Task<byte[]> GenerarExcel(List<AsignacionDTO> asignaciones)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Asignaciones");

                // Encabezados
                var headers = new string[] {
                    "ID Asignación", "Fecha Asignación", "Usuario", "Área", "Departamento",
                    "ID Detalle", "ID Equipo", "Marca Equipo", "Modelo Equipo", "Tipo Equipo",
                    "Velocidad Procesador", "Tipo Procesador", "Memoria RAM", "Tipo Memoria RAM",
                    "ID Disco Duro", "Capacidad Disco Duro", "ID Componente", "Tipo Componente",
                    "Marca Componente", "Modelo Monitor", "Modelo Teléfono", "Idioma Teclado",
                    "ID Dispositivo Ext", "Marca Dispositivo Ext", "Descripción Dispositivo Ext",
                    "ID Equipo Seguridad", "Marca Seguridad", "Modelo Seguridad", "Capacidad Seguridad",
                    "Tipo Seguridad"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headers[i];
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                }

                // Llenado de datos
                int row = 2;
                foreach (var asignacion in asignaciones)
                {
                    foreach (var detalle in asignacion.detalleAsignaciones)
                    {
                        worksheet.Cell(row, 1).Value = asignacion.idAsignacion;
                        worksheet.Cell(row, 2).Value = asignacion.fechaAsignacion;
                        worksheet.Cell(row, 3).Value = asignacion.usuario?.nombreUsuario;
                        worksheet.Cell(row, 4).Value = asignacion.usuario?.area;
                        worksheet.Cell(row, 5).Value = asignacion.usuario?.departamento;

                        worksheet.Cell(row, 6).Value = detalle.idDetalleAsignacion;
                        worksheet.Cell(row, 7).Value = detalle.equipo?.idEquipo;
                        worksheet.Cell(row, 8).Value = detalle.equipo?.marca;
                        worksheet.Cell(row, 9).Value = detalle.equipo?.modelo;
                        worksheet.Cell(row, 10).Value = detalle.equipo?.tipoEquipo;
                        worksheet.Cell(row, 11).Value = detalle.equipo?.velocidadProcesador;
                        worksheet.Cell(row, 12).Value = detalle.equipo?.tipoProcesador;
                        worksheet.Cell(row, 13).Value = detalle.equipo?.memoriaRam;
                        worksheet.Cell(row, 14).Value = detalle.equipo?.tipoMemoriaRam;

                        worksheet.Cell(row, 15).Value = detalle.equipo?.discoDuro?.idDiscoDuro;
                        worksheet.Cell(row, 16).Value = detalle.equipo?.discoDuro?.capacidad;

                        worksheet.Cell(row, 17).Value = detalle.componente?.idComponente;
                        worksheet.Cell(row, 18).Value = detalle.componente?.tipoComponente;
                        worksheet.Cell(row, 19).Value = detalle.componente?.marcaComponente;
                        worksheet.Cell(row, 20).Value = detalle.componente?.modeloMonitor;
                        worksheet.Cell(row, 21).Value = detalle.componente?.modeloTelefono;
                        worksheet.Cell(row, 22).Value = detalle.componente?.idiomaTeclado;

                        worksheet.Cell(row, 23).Value = detalle.dispositivoExt?.idDispExt;
                        worksheet.Cell(row, 24).Value = detalle.dispositivoExt?.marca;
                        worksheet.Cell(row, 25).Value = detalle.dispositivoExt?.descripcion;

                        worksheet.Cell(row, 26).Value = detalle.equipoSeguridad?.idEquipoSeguridad;
                        worksheet.Cell(row, 27).Value = detalle.equipoSeguridad?.marca;
                        worksheet.Cell(row, 28).Value = detalle.equipoSeguridad?.modelo;
                        worksheet.Cell(row, 29).Value = detalle.equipoSeguridad?.capacidad;
                        worksheet.Cell(row, 30).Value = detalle.equipoSeguridad?.tipo;

                        row++;
                    }
                }

                // Ajustar columnas
                worksheet.Columns().AdjustToContents();

                // Guardar en memoria y devolver como array de bytes
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
