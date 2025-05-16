using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using apiFestivos.Aplicacion.Servicios;
using apiFestivos.Core.Interfaces.Repositorios;
using apiFestivos.Dominio.Entidades;

namespace apiFestivos.Test
{
    public class FestivoServicioTest
    {
        private readonly FestivoServicio _service;
        private readonly Mock<IFestivoRepositorio> FestivosRepositorioMock;

        public FestivoServicioTest()
        {
            //  el mock del repositorio
            FestivosRepositorioMock = new Mock<IFestivoRepositorio>();
            //  Inyectamos el mock en el servicio
            _service = new FestivoServicio(FestivosRepositorioMock.Object);
        }

        [Fact]
        public async Task ObtenerFestivo_Tipo1_DeberiaRetornarFechaFija()
        {
            // ARRANGE
            var año = 2025;
            var festivoFijo = new Festivo
            {
                Nombre = "La Independencia ",
                Dia = 1,
                Mes = 1,
                IdTipo = 1,
                DiasPascua = 0
            };
            // Cuando el servicio pida todos los festivos devolve solo nuestro festivo fijo
            FestivosRepositorioMock.Setup(festivos => festivos.ObtenerTodos()).ReturnsAsync(new[] { festivoFijo });

            // ACT
            var resultado = await _service.ObtenerAño(año);
            var primeraFecha = resultado.First().Fecha;

            // ASSERT
            Assert.Equal(new DateTime(2025, 1, 1), primeraFecha);
        }


        [Fact]
        public async Task ObtenerFestivo_Tipo2_DeberiaMoverseAlSiguienteLunes()
        {
            // ARRANGE
            var año = 2025;
            var festivoMovible = new Festivo
            {
                Nombre = "San José",
                Dia = 19,
                Mes = 3,
                IdTipo = 2, // Tipo 2: Ley Puente
                DiasPascua = 0
            };
            FestivosRepositorioMock.Setup(f => f.ObtenerTodos()).ReturnsAsync(new[] { festivoMovible });

            // ACT
            var resultado = await _service.ObtenerAño(año);
            var fechaCalculada = resultado.First().Fecha;

            // ASSERT
            // 19 de marzo de 2025 es miércoles osea lunes siguiente es 24 de marzo de 2025
            Assert.Equal(new DateTime(2025, 3, 24), fechaCalculada);
        }
    }
}
