using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EFCoreEjemplos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            Console.WriteLine("Listo");
        }

        static void EjemploInsertarEstudiante()
        {
            using (var context = new ApplicationDbContext())
            {
                var estudiante = new Estudiante();
                estudiante.Nombre = "Claudia";
                context.Add(estudiante);
                context.SaveChanges();
            }
        }

        static void EjemploActualizarEstudianteModeloConectado()
        {
            using (var context = new ApplicationDbContext())
            {
                var estudiantes = context.Estudiantes.Where(x => x.Nombre == "Claudia").ToList();

                estudiantes[0].Nombre += " Apellido";

                context.SaveChanges();

            }
        }

        static void EjemploActualizarEstudianteModeloDesconectado(Estudiante estudiante)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(estudiante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
        }

        static void EjemploRemoverEstudianteModeloConectado()
        {
            using (var context = new ApplicationDbContext())
            {
                var estudiante = context.Estudiantes.FirstOrDefault();
                context.Remove(context);
                context.SaveChanges();
            }
        }

        static void EjemploRemoverEstudianteModeloDesonectado(Estudiante estudiante)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(estudiante).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                context.SaveChanges();
            }
        }

        static void AgregarModeloUnoAUnoConectado()
        {
            using (var context = new ApplicationDbContext())
            {
                // Aquí agregamos un nuevo estudiante y su dirección
                var estudiante = new Estudiante();
                estudiante.Nombre = "Claudio";
                estudiante.Edad = 99;

                var direccion = new Direccion();
                direccion.Calle = "Ejemplo";
                estudiante.Direccion = direccion;

                context.Add(estudiante);
                context.SaveChanges();
            }
        }

        static void AgregarModeloUnoAUnoModeloDesconectado(Direccion direccion)
        {
            // Modelo desconectado (el campo direccion.EstudianteId debe estar lleno)
            using (var context = new ApplicationDbContext())
            {
                context.Entry(direccion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }

        }

        static void TraerDataRelacionada()
        {
            // Utilizamos include para indicar que queremos traer los estudiantes y sus direcciones
            using (var context = new ApplicationDbContext())
            {
                var estudiantes = context.Estudiantes.Include(x => x.Direccion).ToList();
            }
        }

        static void AgregarModeloMuchosAMuchosModeloDesconectado(Estudiante estudiante)
        {
            // el campo estudiante.InstitucionId debe estar lleno
            using (var context = new ApplicationDbContext())
            {
                context.Add(estudiante);
                context.SaveChanges();
            }
        }

        static void TraerDataRelacionadaUnoAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {

                var institucionesEstudiantes1 = context.Instituciones.Where(x => x.Id == 1).Include(x => x.Estudiantes).ToList();

                // error 
                // var institucion = context.Instituciones.Where(x => x.Id == 1).Include(x => x.Estudiantes.Where(e => e.Edad > 18)).ToList();

                // proyección
                //var persona = context.Estudiantes.Select(x => new { prop =  x.Id, prop1 = x.Nombre }).FirstOrDefault();    

                var institucionesEstudiantes = context.Instituciones.Where(x => x.Id == 1)
                    .Select(x => new { Institucion = x, Estudiantes = x.Estudiantes.Where(e => e.Edad > 18).ToList() }).ToList();

            }
        }

        static void InsertarDataRelacionadaMuchosAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {
                var estudiante = context.Estudiantes.FirstOrDefault();
                var curso = context.Cursos.FirstOrDefault();

                var estudianteCurso = new EstudianteCurso();
                
                estudianteCurso.CursoId = curso.Id;
                estudianteCurso.EstudianteId = estudiante.Id;
                estudianteCurso.Activo = true;

                context.Add(estudianteCurso);
                context.SaveChanges();
            }
        }

        static void TraerDataRelacionadaMuchosAMuchos()
        {
            using (var context = new ApplicationDbContext())
            {
                var curso = context.Cursos.Where(x => x.Id == 1).Include(x => x.EstudiantesCursos)
                    .ThenInclude(y => y.Estudiante).FirstOrDefault();
            }
        }
    }

    class Institucion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Estudiante> Estudiantes { get; set; }
    }

    class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int InstitucionId { get; set; }
        public Direccion Direccion { get; set; }
        public List<EstudianteCurso> EstudiantesCursos { get; set; }
    }

    class Direccion
    {
        public int Id { get; set; }
        public string Calle { get; set; }
        public int EstudianteId { get; set; }
    }

    class Curso
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Nombre { get; set; }
        public List<EstudianteCurso> EstudiantesCursos { get; set; }
    }

    class EstudianteCurso
    {
        public int EstudianteId { get; set; }
        public int CursoId { get; set; }
        public bool Activo { get; set; }
        public Estudiante Estudiante { get; set; }
        public Curso Curso { get; set; }
    }
}
