using System;
using System.Collections.Generic;
using System.Linq;

namespace PracticaLinq;

public class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        bool salir = false;

        while (!salir)
        {
            Console.WriteLine("==============================");
            Console.WriteLine("      PRÁCTICA DE LINQ");
            Console.WriteLine("==============================");
            Console.WriteLine("1. Sucursales con existencias suficientes");
            Console.WriteLine("2. Precios dentro del presupuesto");
            Console.WriteLine("3. Nombres de clientes para un reporte");
            Console.WriteLine("4. Tickets pendientes");
            Console.WriteLine("5. Búsqueda de un código de producto");
            Console.WriteLine("6. Preparar resultados para dos componentes");
            Console.WriteLine("7. Países por código");
            Console.WriteLine("8. Productos y categorías");
            Console.WriteLine("0. Salir");
            Console.Write("Elija una opción: ");

            string? opcion = Console.ReadLine()?.Trim();
            Console.WriteLine();

            switch (opcion)
            {
                case "1": Problema1(); break;
                case "2": Problema2(); break;
                case "3": Problema3(); break;
                case "4": Problema4(); break;
                case "5": Problema5(); break;
                case "6": Problema6(); break;
                case "7": Problema7(); break;
                case "8": Problema8(); break;
                case "0":
                    salir = true;
                    Console.WriteLine("Programa finalizado.");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Escriba un número del 0 al 8.");
                    break;
            }

            if (!salir)
            {
                Console.WriteLine();
                Console.Write("Presione Enter para volver al menú...");
                Console.ReadLine();
                Console.WriteLine();
            }
        }
    }

    // ------------------------------------------------------------
    // 1. Sucursales con existencias suficientes
    // ------------------------------------------------------------
    static void Problema1()
    {
        Console.WriteLine("--- Problema 1: Sucursales con existencias suficientes ---");

        List<int> existencias = new List<int> { 85, 60, 70, 95, 70, 40 };
        int[] pedidos = { 70, 100, 40 };

        Console.WriteLine($"Existencias originales: {string.Join(", ", existencias)}");

        foreach (int cantidadSolicitada in pedidos)
        {
            // Where filtra las sucursales que cubren el pedido completo (conserva orden y repeticiones)
            List<int> suficientes = existencias
                .Where(e => e >= cantidadSolicitada)
                .ToList();

            Console.WriteLine();
            Console.WriteLine($"Pedido de {cantidadSolicitada} unidades:");

            if (suficientes.Count == 0)
            {
                Console.WriteLine("  Ninguna sucursal puede atender el pedido completo.");
            }
            else
            {
                Console.WriteLine($"  Cantidades de sucursales que cumplen: {string.Join(", ", suficientes)}");
                Console.WriteLine($"  Sucursales que cumplen: {suficientes.Count}");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Existencias originales (sin cambios): {string.Join(", ", existencias)}");
    }

    // ------------------------------------------------------------
    // 2. Precios dentro del presupuesto
    // ------------------------------------------------------------
    static void Problema2()
    {
        Console.WriteLine("--- Problema 2: Precios dentro del presupuesto ---");

        List<decimal> precios = new List<decimal> { 25m, 10m, 40m, 15m, 10m, 30m };
        decimal[] presupuestos = { 25m, 5m, 40m };

        Console.WriteLine($"Precios originales: {string.Join(", ", precios.Select(p => p.ToString("F2")))}");

        foreach (decimal presupuesto in presupuestos)
        {
            // Where filtra (<=) y OrderBy ordena de menor a mayor; se conservan repeticiones
            List<decimal> dentro = precios
                .Where(p => p <= presupuesto)
                .OrderBy(p => p)
                .ToList();

            Console.WriteLine();
            Console.WriteLine($"Presupuesto de {presupuesto:F2}:");

            if (dentro.Count == 0)
            {
                Console.WriteLine("  No hay precios dentro del presupuesto.");
            }
            else
            {
                Console.WriteLine($"  Precios (de menor a mayor): {string.Join(", ", dentro.Select(p => p.ToString("F2")))}");
                Console.WriteLine($"  Cantidad: {dentro.Count}");
            }
        }
    }

    // ------------------------------------------------------------
    // 3. Nombres de clientes para un reporte
    // ------------------------------------------------------------
    static void Problema3()
    {
        Console.WriteLine("--- Problema 3: Nombres de clientes para un reporte ---");

        List<Cliente> clientes = new List<Cliente>
        {
            new Cliente(101, "Ana"),
            new Cliente(102, "Roberto"),
            new Cliente(103, "Luis"),
            new Cliente(104, "Ana")
        };

        // Variantes: se trabaja sobre copias para conservar la lista original
        List<Cliente> conMaria = clientes.Concat(new[] { new Cliente(105, "María José") }).ToList();
        List<Cliente> vacia = new List<Cliente>();

        void Reporte(string titulo, List<Cliente> origen)
        {
            // Select proyecta solo el nombre; un nombre por cliente, en el orden original
            List<string> nombres = origen.Select(c => c.Nombre).ToList();

            Console.WriteLine();
            Console.WriteLine(titulo);

            if (nombres.Count == 0)
            {
                Console.WriteLine("  No hay clientes para el reporte.");
            }
            else
            {
                Console.WriteLine($"  Nombres: {string.Join(", ", nombres)}");
            }
            Console.WriteLine($"  Cantidad: {nombres.Count}");
        }

        Reporte("Caso inicial:", clientes);
        Reporte("Con el cliente 105 \"María José\" agregado:", conMaria);
        Reporte("Con lista de clientes vacía:", vacia);

        Console.WriteLine();
        Console.WriteLine($"Clientes originales (sin cambios): {clientes.Count}");
    }

    // ------------------------------------------------------------
    // 4. Tickets pendientes
    // ------------------------------------------------------------
    static void Problema4()
    {
        Console.WriteLine("--- Problema 4: Tickets pendientes ---");

        List<Ticket> tickets = new List<Ticket>
        {
            new Ticket("Instalar editor", "Pendiente", 1),
            new Ticket("Restablecer red", "Pendiente", 3),
            new Ticket("Revisar impresora", "Cerrado", 3),
            new Ticket("Configurar correo", "Pendiente", 2)
        };

        // Variante: copia con un ticket pendiente de prioridad alta y otro cerrado
        List<Ticket> ampliados = tickets.Concat(new[]
        {
            new Ticket("Reparar servidor", "Pendiente", 3),
            new Ticket("Archivar documentos", "Cerrado", 2)
        }).ToList();

        void Buscar(string titulo, List<Ticket> origen, string estadoBuscado)
        {
            // Where por estado, OrderByDescending por prioridad (3 alta -> 1 baja) y Select del título
            List<string> titulos = origen
                .Where(t => t.Estado == estadoBuscado)
                .OrderByDescending(t => t.Prioridad)
                .Select(t => t.Titulo)
                .ToList();

            Console.WriteLine();
            Console.WriteLine($"{titulo} (estado buscado: \"{estadoBuscado}\")");

            if (titulos.Count == 0)
            {
                Console.WriteLine("  No hay tickets con ese estado.");
            }
            else
            {
                Console.WriteLine($"  Títulos (de mayor a menor prioridad): {string.Join(", ", titulos)}");
                Console.WriteLine($"  Cantidad: {titulos.Count}");
            }
        }

        Buscar("Caso inicial:", tickets, "Pendiente");
        Buscar("Con un ticket pendiente alto y otro cerrado agregados:", ampliados, "Pendiente");
        Buscar("Estado que no aparece en los datos:", ampliados, "En proceso");
    }

    // ------------------------------------------------------------
    // 5. Búsqueda de un código de producto
    // ------------------------------------------------------------
    static void Problema5()
    {
        Console.WriteLine("--- Problema 5: Búsqueda de un código de producto ---");

        List<string> codigos = new List<string> { "P-100", "P-200", "P-300", "P-100" };
        List<string> vacia = new List<string>();

        var fuentes = new (string Nombre, List<string> Datos)[]
        {
            ("Importación con datos", codigos),
            ("Colección vacía", vacia)
        };
        string[] buscados = { "P-100", "P-999" };

        foreach (var fuente in fuentes)
        {
            foreach (string codigoBuscado in buscados)
            {
                Console.WriteLine();
                Console.WriteLine($"{fuente.Nombre} - buscando \"{codigoBuscado}\":");

                // Versión 1: First lanza InvalidOperationException si no hay coincidencias.
                // Con duplicados devuelve únicamente la primera coincidencia.
                try
                {
                    string encontrado = fuente.Datos.First(c => c == codigoBuscado);
                    Console.WriteLine($"  Versión 1 (First): código encontrado -> {encontrado}");
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("  Versión 1 (First): se produjo una excepción; el código no existe.");
                }

                // Versión 2: FirstOrDefault devuelve null si no hay coincidencias, sin excepción.
                string? resultado = fuente.Datos.FirstOrDefault(c => c == codigoBuscado);
                if (resultado != null)
                {
                    Console.WriteLine($"  Versión 2 (FirstOrDefault): código encontrado -> {resultado}");
                }
                else
                {
                    Console.WriteLine("  Versión 2 (FirstOrDefault): el código no existe.");
                }
            }
        }
    }

    // ------------------------------------------------------------
    // 6. Preparar resultados para dos componentes
    // ------------------------------------------------------------
    static void Problema6()
    {
        Console.WriteLine("--- Problema 6: Preparar resultados para dos componentes ---");

        List<int> edades = new List<int> { 15, 22, 17, 30, 18 };
        List<int> soloMenores = new List<int> { 15, 17, 10, 12 };

        void Procesar(string titulo, List<int> original)
        {
            Console.WriteLine();
            Console.WriteLine(titulo);
            Console.WriteLine($"  Colección original: {Formato(original)}");

            // Se crean dos resultados independientes a partir del mismo filtro
            List<int> lista = original.Where(e => e >= 18).ToList();   // componente que puede ampliarse
            int[] arreglo = original.Where(e => e >= 18).ToArray();    // componente que recibe un arreglo

            Console.WriteLine("  Antes de modificar:");
            Console.WriteLine($"    Lista:   {Formato(lista)} (elementos: {lista.Count})");
            Console.WriteLine($"    Arreglo: {Formato(arreglo)} (elementos: {arreglo.Length})");

            lista.Add(40);

            if (arreglo.Length > 0)
            {
                arreglo[0] = 23;
            }
            else
            {
                Console.WriteLine("  El arreglo está vacío: no se puede cambiar su primer elemento.");
            }

            Console.WriteLine("  Después de modificar:");
            Console.WriteLine($"    Lista:   {Formato(lista)} (elementos: {lista.Count})");
            Console.WriteLine($"    Arreglo: {Formato(arreglo)} (elementos: {arreglo.Length})");
            Console.WriteLine($"    Original (sin cambios): {Formato(original)}");
        }

        string Formato(IEnumerable<int> valores)
        {
            return valores.Any() ? string.Join(", ", valores) : "(vacío)";
        }

        Procesar("Caso inicial:", edades);
        Procesar("Fuente sin personas mayores de edad:", soloMenores);
    }

    // ------------------------------------------------------------
    // 7. Países por código
    // ------------------------------------------------------------
    static void Problema7()
    {
        Console.WriteLine("--- Problema 7: Países por código ---");

        List<Pais> paises = new List<Pais>
        {
            new Pais("PA", "Panamá"),
            new Pais("PE", "Perú"),
            new Pais("CL", "Chile")
        };

        // ToDictionary: clave = código, valor = nombre
        Dictionary<string, string> diccionario = paises.ToDictionary(p => p.Codigo, p => p.Nombre);

        Console.WriteLine("Diccionario creado:");
        foreach (KeyValuePair<string, string> par in diccionario)
        {
            Console.WriteLine($"  {par.Key} -> {par.Value}");
        }

        string[] consultas = { "PA", "MX" };
        Console.WriteLine();

        foreach (string codigo in consultas)
        {
            // TryGetValue evita la excepción KeyNotFoundException
            if (diccionario.TryGetValue(codigo, out string? nombre))
            {
                Console.WriteLine($"Código \"{codigo}\": {nombre}");
            }
            else
            {
                Console.WriteLine($"Código \"{codigo}\": no existe en el diccionario.");
            }
        }
    }

    // ------------------------------------------------------------
    // 8. Productos y categorías
    // ------------------------------------------------------------
    static void Problema8()
    {
        Console.WriteLine("--- Problema 8: Productos y categorías ---");

        List<Categoria> categorias = new List<Categoria>
        {
            new Categoria(1, "Papelería"),
            new Categoria(2, "Tecnología"),
            new Categoria(3, "Hogar")
        };

        List<Producto> productos = new List<Producto>
        {
            new Producto("Cuaderno", 1),
            new Producto("Ratón", 2),
            new Producto("Lápiz", 1),
            new Producto("Artículo especial", 99)
        };

        // Variantes sobre copias, para conservar los datos originales
        List<Categoria> conEspeciales = categorias.Concat(new[] { new Categoria(99, "Especiales") }).ToList();
        List<Categoria> conUtiles = conEspeciales.Concat(new[] { new Categoria(1, "Útiles") }).ToList();

        void Relacionar(string titulo, List<Categoria> cats)
        {
            // Join (inner join): solo parejas con Id coincidente; respeta el orden de los productos
            // y conserva todas las coincidencias cuando una categoría se repite.
            List<string> parejas = productos
                .Join(cats,
                      p => p.CategoriaId,
                      c => c.Id,
                      (p, c) => $"{p.Nombre} - {c.Nombre}")
                .ToList();

            Console.WriteLine();
            Console.WriteLine(titulo);

            if (parejas.Count == 0)
            {
                Console.WriteLine("  No hay parejas coincidentes.");
            }
            else
            {
                foreach (string pareja in parejas)
                {
                    Console.WriteLine($"  {pareja}");
                }
            }
            Console.WriteLine($"  Cantidad de parejas: {parejas.Count}");
        }

        Relacionar("Caso inicial:", categorias);
        Relacionar("Con la categoría 99 \"Especiales\":", conEspeciales);
        Relacionar("Con otra categoría de identificador 1 (\"Útiles\"):", conUtiles);
    }
}
