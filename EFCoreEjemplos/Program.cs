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

        // Usar este método para llenar la base de datos con data de prueba
        static void SeedDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Database.Migrate();

                if (context.Instituciones.Any())
                {
                    // Si ya hay data, no hacer nada.
                    return;
                }


                var institucion1 = new Institucion();
                institucion1.Nombre = "Institucion 1";

                var estudiante1 = new Estudiante();
                estudiante1.Nombre = "Felipe";
                estudiante1.Edad = 999;

                var estudiante2 = new Estudiante();
                estudiante2.Nombre = "Claudia";
                estudiante2.Edad = 15;

                var estudiante3 = new Estudiante();
                estudiante3.Nombre = "Roberto";
                estudiante3.Edad = 25;

                var direccion1 = new Direccion();
                direccion1.Calle = "Avenida Siempreviva 123";
                estudiante1.Direccion = direccion1;

                var curso1 = new Curso();
                curso1.Nombre = "Calculo";

                var curso2 = new Curso();
                curso2.Nombre = "Algebra Lineal";

                var institucion2 = new Institucion();
                institucion2.Nombre = "Institucion 2";

                institucion1.Estudiantes.Add(estudiante1);
                institucion1.Estudiantes.Add(estudiante2);

                institucion2.Estudiantes.Add(estudiante3);

                context.Add(institucion1);
                context.Add(institucion2);
                context.Add(curso1);
                context.Add(curso2);

                context.SaveChanges();

                var estudianteCurso = new EstudianteCurso();
                estudianteCurso.Activo = true;
                estudianteCurso.CursoId = curso1.Id;
                estudianteCurso.EstudianteId = estudiante1.Id;

                var estudianteCurso2 = new EstudianteCurso();
                estudianteCurso2.Activo = false;
                estudianteCurso2.CursoId = curso1.Id;
                estudianteCurso2.EstudianteId = estudiante2.Id;

                context.Add(estudianteCurso);
                context.Add(estudianteCurso2);
                context.SaveChanges();
            }
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

        static void StringInterpolationEnEF2()
        {
            using (var context = new ApplicationDbContext())
            {
                var nombre = "'Felipe' or 1=1";
                // Así evitamos SQL Injection
                var estudiante = context.Estudiantes.FromSql($"select * from Estudiantes where Nombre = {nombre}").ToList();
            }
        }

        static void FiltroPorTipo()
        {
            using (var context = new ApplicationDbContext())
            {
                // El filtro definido en el ApplicationDbContext.cs se aplica automáticamente
                var estudiantesCursos = context.EstudiantesCursos.ToList();
            }
        }

        static void BorradoSuave()
        {
            // El estudiante no será borrado (Ver SaveChanges en ApplicationDbContext
            using (var context = new ApplicationDbContext())
            {
                var estudiante = context.Estudiantes.FirstOrDefault();
                context.Remove(estudiante);
                context.SaveChanges();
            }
        }

        static void EjemploConcurrencyCheck()
        {
            using (var context = new ApplicationDbContext())
            {
                var est = context.Estudiantes.FirstOrDefault();
                est.Nombre += " 2";
                est.Edad += 1;
                context.SaveChanges();
            }
        }

    }

    class Institucion
    {
        public Institucion()
        {
            Estudiantes = new List<Estudiante>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Estudiante> Estudiantes { get; set; }
    }

    class Estudiante
    {
        public int Id { get; set; }
        [ConcurrencyCheck]
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int InstitucionId { get; set; }
        public bool EstaBorrado { get; set; }
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
