public static class DotEnv
{
    // Método estático para cargar variables de entorno desde un archivo .env
    public static void Load(string filePath)
    {
        // Verifica si el archivo existe; si no, simplemente retorna
        if (!File.Exists(filePath))
            return;

        // Lee todas las líneas del archivo .env
        foreach (var line in File.ReadAllLines(filePath))
        {
            // Divide cada línea en dos partes, separadas por el símbolo '='
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);

            // Si la línea no tiene exactamente dos partes (clave y valor), la omite
            if (parts.Length != 2)
                continue;

            // Establece la variable de entorno con la clave y valor obtenidos
            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}
