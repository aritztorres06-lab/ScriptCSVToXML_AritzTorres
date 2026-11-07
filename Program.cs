using System;
using System.IO;
using System.Text;
using System.Xml;

string rutaCSV = "./NetflixOriginals.csv";
string rutaXML = "./NetflixOriginals.xml";
int opcion = 0;

while (opcion != 3)
{
    Console.Clear();
    Console.WriteLine("=== MENÚ FICHEROS ===");
    Console.WriteLine("1. De CSV a XML");
    Console.WriteLine("2. De XML a CSV");
    Console.WriteLine("3. Salir");
    Console.Write("\nSelecciona una opción: ");
    
    try
    {
        opcion = int.Parse(Console.ReadLine());
    }
    catch
    {
        opcion = 0;
    }
    
    switch (opcion)
    {
        case 1:
            if (ValidarArchivo(rutaCSV))
            {
                SubmenuCSVaXML();
            }
            break;

        case 2:
            if (ValidarArchivo(rutaXML))
            {
                SubmenuXMLaCSV();
            }
            break;

        case 3:
            Console.WriteLine("\n¡Hasta luego!");
            break;

        default:
            Console.WriteLine("\nOpción no válida. Debes introducir un número del menú.");
            Console.WriteLine("Presiona [Enter] para intentarlo de nuevo.");
            Console.ReadLine();
            break;
    }
}

void SubmenuCSVaXML()
{
    
    
    int opcion = 0;
    while (opcion != 5)
    {
        Console.Clear();
        Console.WriteLine("--- EXPORTAR DE CSV A XML ---");
        Console.WriteLine("1. Pasar todo el documento CSV a XML");
        Console.WriteLine("2. Exportar por género");
        Console.WriteLine("3. Exportar por año");
        Console.WriteLine("4. Exportar por lenguaje");
        Console.WriteLine("5. Volver al menú principal");
        Console.Write("\nSelecciona una opción: ");

        try
        {
            opcion = int.Parse(Console.ReadLine());
        }
        catch
        {
            opcion = 0;
        }

        switch (opcion)
        {
            case 1:
                exportarCSVaXML(rutaCSV, rutaXML);
                Pausar();
                break;
            case 2:
                exportarGeneroCSVaXML();
                Pausar();
                break;
            case 3:
                exportarAñoCSVaXML();
                Pausar();
                break;
            case 4:
                exportarLenguajeCSVaXML();
                Pausar();
                break;
            case 5:
                break;
                
            default:
                Console.WriteLine("\nOpción no válida. Debes introducir un número del 1 al 5.");
                Console.WriteLine("Presiona [Enter] para intentarlo de nuevo.");
                Console.ReadLine();
                break;
        }
    }
}

void SubmenuXMLaCSV()
{
    int opcion = 0;
    while (opcion != 5)
    {
        Console.Clear();
        Console.WriteLine("--- EXPORTAR DE XML A CSV ---");
        Console.WriteLine("1. Pasar todo el documento XML a CSV");
        Console.WriteLine("2. Exportar por género");
        Console.WriteLine("3. Exportar por año");
        Console.WriteLine("4. Exportar por lenguaje");
        Console.WriteLine("5. Volver al menú principal");
        Console.Write("\nSelecciona una opción: ");

        try
        {
            opcion = int.Parse(Console.ReadLine());
        }
        catch
        {
            opcion = 0;
        }

        switch (opcion)
        {
            case 1:
                exportarXMLaCSV(rutaXML, rutaCSV);
                Pausar();
                break;
            case 2:
                exportarGeneroXMLaCSV();
                Pausar();
                break;
            case 3:
                exportarAñoXMLaCSV();
                Pausar();
                break;
            case 4:
                exportarLenguajeXMLaCSV();
                Pausar();
                break;
            
            case 5:
                break;

            default:
                Console.WriteLine("\nOpción no válida. Presiona [Enter] para intentarlo de nuevo");
                Console.ReadLine();
                break;
        }
    }
}

void Pausar()
{
    Console.WriteLine("\nPresiona Enter para continuar...");
    Console.ReadLine();
}

bool ValidarArchivo(string ruta)
{
    if (!File.Exists(ruta))
    {
        Console.WriteLine($"\nEl archivo no existe en la ruta ({ruta}).");
        Pausar();
        return false;
    }
    return true;
}

void escribirNodoPelicula(XmlWriter xmlWriter, string[] campos)
{
    xmlWriter.WriteStartElement("Pelicula");

    xmlWriter.WriteStartElement("Title");
    xmlWriter.WriteString(campos[0]);
    xmlWriter.WriteEndElement();

    xmlWriter.WriteStartElement("Genre");
    xmlWriter.WriteString(campos[1]);
    xmlWriter.WriteEndElement();

    xmlWriter.WriteStartElement("Premiere");
    xmlWriter.WriteString((campos[2] + "," + campos[3]).Replace("\"", ""));
    xmlWriter.WriteEndElement();

    xmlWriter.WriteStartElement("Runtime");
    xmlWriter.WriteString(campos[4]);
    xmlWriter.WriteEndElement();

    xmlWriter.WriteStartElement("IMDB_Score");
    xmlWriter.WriteString(campos[5]);
    xmlWriter.WriteEndElement();

    xmlWriter.WriteStartElement("Language");
    xmlWriter.WriteString(campos[6]);
    xmlWriter.WriteEndElement();

    xmlWriter.WriteEndElement();
}

void exportarCSVaXML(string rutaCSV, string rutaXML)
{
    String[] lineas = File.ReadAllLines(rutaCSV).ToArray();
    
    XmlWriterSettings settings = new XmlWriterSettings();
    settings.Indent = true;

    XmlWriter xmlWriter = XmlWriter.Create(rutaXML, settings);
    
    xmlWriter.WriteStartDocument();
    xmlWriter.WriteStartElement("Peliculas");

    for(int i = 1; i < lineas.Length; i++)
    {
        string[] campos = lineas[i].Split(',');

        if(campos.Length >= 7)
        {
            escribirNodoPelicula(xmlWriter, campos);
        }
    }
    
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Close();

    Console.WriteLine("¡Se ha exportado todo el documento CSV a XML con éxito!");
}

void exportarGeneroCSVaXML()
{
    String[] lineas = File.ReadAllLines(rutaCSV).ToArray();

    List<string> generos = new List<string>();
    
    for (int i = 1; i < lineas.Length; i++)
    {
        string[] campos = lineas[i].Split(',');

        if (campos.Length >= 7)
        {
            string genero = campos[1].Trim('"', ' ');
            if (!generos.Contains(genero))
            {
                generos.Add(genero);
            }
        }
    }

    Console.WriteLine("Lista de géneros: ");
    foreach (string genero in generos)
    {
        Console.WriteLine("- " + genero);
    }

    Console.Write("\nEscribe el género que quieres exportar: ");
    string generoElegido = Console.ReadLine();

    if (!generos.Contains(generoElegido))
    {
        Console.WriteLine("\nEl género que has introducido no existe.");
        return;
    }

    XmlWriterSettings settings = new XmlWriterSettings();
    settings.Indent = true;

    XmlWriter xmlWriter = XmlWriter.Create(rutaXML, settings);
    
    xmlWriter.WriteStartDocument();
    xmlWriter.WriteStartElement("Peliculas");

    for(int i = 1; i < lineas.Length; i++)
    {
        string[] campos = lineas[i].Split(',');

        if(campos.Length >= 7)
        {
            if(campos[1] == generoElegido)
            {
                escribirNodoPelicula(xmlWriter,campos);
            }
        }
    }
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Close();

    Console.WriteLine("¡Se han exportado las películas del género seleccionado!");
}

void exportarAñoCSVaXML()
{
    Console.Write("Introduce el año: ");
    int año = int.Parse(Console.ReadLine());

    String[] lineas = File.ReadAllLines(rutaCSV).ToArray();

    bool encontrado = false;

    XmlWriterSettings settings = new XmlWriterSettings();
    settings.Indent = true;

    XmlWriter xmlWriter = XmlWriter.Create(rutaXML, settings);

    xmlWriter.WriteStartDocument();
    xmlWriter.WriteStartElement("Peliculas");

    for (int i = 1; i < lineas.Length; i++)
    {
        string[] campos = lineas[i].Split(',');

        if (campos.Length >= 7)
        {
            int añoPelicula = 0;

            foreach (string campo in campos)
            {
                string texto = campo.Replace("\"", "").Trim();

                if (texto.Length == 4)
                {
                    try
                    {
                        añoPelicula = int.Parse(texto);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("No es un año válido");
                    }
                }
            }

            if (añoPelicula >= año)
            {
                escribirNodoPelicula(xmlWriter, campos);
                encontrado = true;
            }
        }
    }

    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Close();

    if (encontrado)
    {
        Console.WriteLine("\n¡Se han exportado las películas de ese año y superiores!");
    }
    else
    {
        Console.WriteLine($"\nNo existe ninguna película a partir del año {año}.");
    }
}

void exportarLenguajeCSVaXML()
{   
    String[] lineas = File.ReadAllLines(rutaCSV).ToArray();

    List<string> lenguajes = new List<string>();
    
    for (int i = 1; i < lineas.Length; i++)
    {
        string[] campos = lineas[i].Split(',');

        if (campos.Length >= 7)
        {
            int ultimaPosicion = campos.Length - 1;
            string lenguaje = campos[ultimaPosicion].Trim('"', ' ');
            
            if (!lenguajes.Contains(lenguaje))
            {
                lenguajes.Add(lenguaje);
            }
        }
    }

    Console.WriteLine("Lista de lenguajes: ");
    foreach (string lenguaje in lenguajes)
    {
        Console.WriteLine("- " + lenguaje);
    }

    Console.Write("\nEscribe el lenguaje que quieres exportar: ");
    string lenguajeElegido = Console.ReadLine();

    if (!lenguajes.Contains(lenguajeElegido))
    {
        Console.WriteLine("\nEl lenguaje que has introducido no existe.");
        return;
    }

    XmlWriterSettings settings = new XmlWriterSettings();
    settings.Indent = true;

    XmlWriter xmlWriter = XmlWriter.Create(rutaXML, settings);
    
    xmlWriter.WriteStartDocument();
    xmlWriter.WriteStartElement("Peliculas");

    for (int i = 1; i < lineas.Length; i++)
    {
        string[] campos = lineas[i].Split(',');

        if (campos.Length >= 7)
        {
            int ultimaPosicion = campos.Length - 1;
            string lenguajeActual = campos[ultimaPosicion].Trim('"', ' ');
            
            if (lenguajeActual == lenguajeElegido)
            {
                escribirNodoPelicula(xmlWriter, campos);
            }
        }
    }
    xmlWriter.WriteEndElement();
    xmlWriter.WriteEndDocument();
    xmlWriter.Close();

    Console.WriteLine("¡Se han exportado las películas del lenguaje seleccionado!");
}

List<string> LeerTodasLasPeliculasCSV(string rutaXML)
{
    List<string> lineas = new List<string>();

    if (!File.Exists(rutaXML))
    {
        Console.WriteLine($"La ruta ({rutaXML}) no existe.");
        return lineas;
    }

    XmlReader xmlReader = XmlReader.Create(rutaXML);
    
    string title = "";
    string genre = ""; 
    string premiere = ""; 
    string runtime = "";
    string imdb = "";
    string language = "";

    while (xmlReader.Read())
    {
        if (xmlReader.NodeType == XmlNodeType.Element)
        {
            switch (xmlReader.Name)
            {
                case "Title":
                    xmlReader.Read();
                    title = xmlReader.Value;
                    break;
                case "Genre":
                    xmlReader.Read();
                    genre = xmlReader.Value;
                    break;
                case "Premiere":
                    xmlReader.Read();
                    premiere = xmlReader.Value;
                    break;
                case "Runtime":
                    xmlReader.Read();
                    runtime = xmlReader.Value;
                    break;
                case "IMDB_Score":
                    xmlReader.Read();
                    imdb = xmlReader.Value;
                    break;
                case "Language":
                    xmlReader.Read();
                    language = xmlReader.Value;
                    break;
            }
        }

        if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "Pelicula")
        {
            lineas.Add($"{title},{genre},\"{premiere}\",{runtime},{imdb},{language}");
        }
    }

    xmlReader.Close();
    return lineas;
}

void exportarXMLaCSV(string rutaXML, string rutaCSV)
{
    List<string> lineas = LeerTodasLasPeliculasCSV(rutaXML);

    StringBuilder sb = new StringBuilder();
    sb.AppendLine("Title,Genre,Premiere,Runtime,IMDB Score,Language");

    foreach (string linea in lineas)
    {
        sb.AppendLine(linea);
    }

    File.WriteAllText(rutaCSV, sb.ToString());
    Console.WriteLine("¡Se ha exportado todo el documento XML a CSV con éxito!");
}

void exportarGeneroXMLaCSV()
{
    List<string> lineas = LeerTodasLasPeliculasCSV(rutaXML);

    List<string> generos = new List<string>();

    foreach (string linea in lineas)
    {
        string[] campos = linea.Split(',');
        
        string genero = campos[1].Trim('"', ' ');
        if (!generos.Contains(genero))
        {
            generos.Add(genero);
        }
    }

    Console.WriteLine("Lista de géneros disponibles:");
    foreach (string genero in generos)
    {
        Console.WriteLine("- " + genero);
    }

    Console.Write("\nEscribe el género que quieres exportar: ");
    string generoElegido = Console.ReadLine();

    if (!generos.Contains(generoElegido))
    {
        Console.WriteLine("\nEl género que has introducido no existe.");
        return;
    }

    StringBuilder sb = new StringBuilder();
    sb.AppendLine("Title,Genre,Premiere,Runtime,IMDB Score,Language");

    foreach (string linea in lineas)
    {
        string[] campos = linea.Split(',');

        string genero = campos[1].Trim('"', ' ');
        if (genero == generoElegido)
        {
            sb.AppendLine(linea);
        }
    }

    File.WriteAllText(rutaCSV, sb.ToString());
    Console.WriteLine("\n¡Se han exportado las películas del género seleccionado!");
}

void exportarAñoXMLaCSV()
{
    Console.WriteLine("Introduce el año que quieres exportar: ");
    int año = int.Parse(Console.ReadLine());

    List<string> lineas = LeerTodasLasPeliculasCSV(rutaXML);

    bool encontrado = false;

    StringBuilder sb = new StringBuilder();
    sb.AppendLine("Title,Genre,Premiere,Runtime,IMDB Score,Language");

    foreach (string linea in lineas)
    {
        string[] campos = linea.Split(',');
        
        string fecha = campos[2].Replace("\"", "");
        int añoPeli = 0;

        foreach (string campo in campos)
            {
                string texto = campo.Replace("\"", "").Trim();

                if (texto.Length == 4)
                {
                    try
                    {
                        añoPeli = int.Parse(texto);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("No es un año válido");
                    }
                }
            }

        if (añoPeli >= año)
        {
            sb.AppendLine(linea);
            encontrado = true;
        }
    }       

    File.WriteAllText(rutaCSV, sb.ToString());

    if (encontrado)
    {
        Console.WriteLine("\n¡Se han exportado las películas de ese año y superiores!");
    }
    else
    {
        Console.WriteLine($"\nNo existe ninguna película a partir del año {año}.");
    }
}

void exportarLenguajeXMLaCSV()
{
    List<string> lineas = LeerTodasLasPeliculasCSV(rutaXML);

    List<string> lenguajes = new List<string>();

    bool primeraLinea = true;
    foreach (string linea in lineas)
    {
        if (primeraLinea)
        {
            primeraLinea = false;
            continue;
        }

        string[] campos = linea.Split(',');
        
        int ultimaPosicion = campos.Length - 1;
        string lenguaje = campos[ultimaPosicion].Trim('"', ' ');

        try
        {
            double.Parse(lenguaje);
        }
        catch
        {
            if (!lenguajes.Contains(lenguaje))
            {
                lenguajes.Add(lenguaje);
            }
        }
    }

    Console.WriteLine("Lista de lenguajes disponibles:");
    foreach (string lenguaje in lenguajes)
    {
        Console.WriteLine("- " + lenguaje);
    }

    Console.Write("\nEscribe el lenguaje que quieres exportar: ");
    string lenguajeElegido = Console.ReadLine();

    if (!lenguajes.Contains(lenguajeElegido))
    {
        Console.WriteLine("\nEl lenguaje que has introducido no existe.");
        return;
    }

    StringBuilder sb = new StringBuilder();
    sb.AppendLine("Title,Genre,Premiere,Runtime,IMDB Score,Language");

    primeraLinea = true;
    foreach (string linea in lineas)
    {
        if (primeraLinea)
        {
            primeraLinea = false;
            continue;
        }

        string[] campos = linea.Split(',');

        int ultimaPosicion = campos.Length - 1;
        string lenguaje = campos[ultimaPosicion].Trim('"', ' ');

        if (lenguaje == lenguajeElegido)
        {
            sb.AppendLine(linea);
        }
    }

    File.WriteAllText(rutaCSV, sb.ToString());
    Console.WriteLine("¡Se han exportado las películas del lenguaje seleccionado!");
}