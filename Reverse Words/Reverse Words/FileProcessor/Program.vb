Imports System
Imports System.IO
Module Program
    Sub Main(args As String())
        Console.WriteLine("Procesador de Archivos: .txt, .csv, .xml")
        Console.WriteLine("---------------------------------------------------")

        ' Pedir la ruta del directorio
        Console.WriteLine("Escriba la ruta completa de la carpeta donde están los archivos:")
        Dim folderPath As String = Console.ReadLine()

        ' Verificar si la carpeta existe
        If Not Directory.Exists(folderPath) Then
            Console.WriteLine("La carpeta no existe. Inténtelo de nuevo.")
            Return
        End If

        ' Obtener y mostrar los archivos disponibles en la carpeta
        Dim supportedExtensions = New String() {".txt", ".in"}
        Dim availableFiles = Directory.GetFiles(folderPath).Where(Function(file) supportedExtensions.Contains(Path.GetExtension(file).ToLower())).ToList()

        If availableFiles.Count = 0 Then
            Console.WriteLine("No se encontraron archivos compatibles en la carpeta.")
            Return
        End If

        Console.WriteLine("Archivos disponibles:")
        For Each file In availableFiles
            Console.WriteLine($"- {Path.GetFileName(file)}")
        Next

        ' Pedir al usuario que elija un archivo
        Console.WriteLine("---------------------------------------------------")
        Console.WriteLine("Escriba el nombre del archivo que desea procesar (incluyendo la extensión):")
        Dim fileName As String = Console.ReadLine()

        ' Verificar si el archivo ingresado existe en la lista
        Dim filePath As String = Path.Combine(folderPath, fileName)
        If Not availableFiles.Contains(filePath) Then
            Console.WriteLine("El archivo no existe o no es compatible. Inténtelo de nuevo.")
            Return
        End If
        
        ' Obtener la extensión del archivo y procesarlo según su tipo
        Dim fileExtension As String = Path.GetExtension(filePath).ToLower()
        Select Case fileExtension
            Case ".txt"
                ProcessTextFile(filePath)
            Case ".in"
                ProcessInFile(filePath)
            Case Else
                Console.WriteLine("Formato de archivo no compatible.")
        End Select

        Console.WriteLine("Procesamiento finalizado. Presione cualquier tecla para salir.")
        Console.ReadKey()


    End Sub

    Sub ProcessTextFile(filePath As String)
        Console.WriteLine("Procesando archivo de texto...")
        Dim lines = File.ReadAllLines(filePath)
        Dim modifiedLines = lines.Select(Function(line) "Modificado: " & line).ToList()
        Dim outputFile = Path.Combine(Path.GetDirectoryName(filePath), "Modificado_" & Path.GetFileName(filePath))
        File.WriteAllLines(outputFile, modifiedLines)
        Console.WriteLine($"Archivo de texto modificado guardado en: {outputFile}")
    End Sub

    Sub ProcessInFile(filePath As String)
        Try
            Console.WriteLine("Procesando archivo .in...")
        ' Leer todas las líneas del archivo
            Dim lines = File.ReadAllLines(filePath)
            Dim outputLines As New List(Of String)
        ' Filtrar y procesar solo líneas que no son números
            Dim caseNumber As Integer = 1
            For Each line In lines
            ' Verificar si la línea contiene texto (no es un número)
                If Not IsNumeric(line) AndAlso Not String.IsNullOrWhiteSpace(line) Then
                ' Dividir la línea en palabras, invertir el orden y unirlas nuevamente
                    Dim reversedWords = line.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries).Reverse()
                    Dim formattedLine = $"Case #{caseNumber}: {String.Join(" ", reversedWords)}"
                ' Agregar la línea procesada a la lista de salida
                    outputLines.Add(formattedLine)
                    caseNumber += 1
                End If
            Next
        ' Generar archivo de salida
            Dim outputFile = Path.Combine(Path.GetDirectoryName(filePath), "Output_" & Path.GetFileName(filePath))
            File.WriteAllLines(outputFile, outputLines)
            Console.WriteLine($"Archivo procesado y guardado en: {outputFile}")
        Catch ex As Exception
            Console.WriteLine($"Error al procesar el archivo: {ex.Message}")
        End Try
    End Sub

End Module

