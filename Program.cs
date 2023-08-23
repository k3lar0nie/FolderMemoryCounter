class FolderMemoryCounter
{
    static List<string> names = new List<string>();
    static List<long> sizes = new List<long>();
    
    private static void Main()
    {
        Console.WriteLine("Choose mode: 1.Get size of folder; 2. Get size of files and folders inside certain folder;");
        
        var mode = Console.ReadKey();
        Console.WriteLine("");
        
        again:
        if (mode.Key == ConsoleKey.D1)
        {
            var path = Console.ReadLine();
            Console.WriteLine($"{DateTime.Now.Hour}h:{DateTime.Now.Minute}m:{DateTime.Now.Second}s:{DateTime.Now.Millisecond}ms");
            Console.WriteLine($"{CountFolderSize(path!) / 1048576}mb");
            Console.WriteLine($"{DateTime.Now.Hour}h:{DateTime.Now.Minute}m:{DateTime.Now.Second}s:{DateTime.Now.Millisecond}ms");
        } 
        else if (mode.Key == ConsoleKey.D2)
        {
            Console.Write("Type in the path to the folder : ");
            var path = Console.ReadLine();
            
            Console.WriteLine($"{DateTime.Now.Hour}h:{DateTime.Now.Minute}m:{DateTime.Now.Second}s:{DateTime.Now.Millisecond}ms");

            DirectoryInfo directoryInfo = new DirectoryInfo(path!);
            foreach (var x in directoryInfo.GetDirectories())
            {
                names.Add($"{x.Name}[Folder]");
                sizes.Add(CountFolderSize(x.FullName));
                
            }
            foreach (var x in directoryInfo.GetFiles())
            {
                names.Add(x.Name);
                sizes.Add(x.Length);
            }
            Console.WriteLine($"{DateTime.Now.Hour}h:{DateTime.Now.Minute}m:{DateTime.Now.Second}s:{DateTime.Now.Millisecond}ms");
            
            Console.WriteLine("Sort by... 1.Size; 2.Name");
            var sortMode = Console.ReadKey();
            Console.WriteLine();
            
            if (sortMode.Key == ConsoleKey.D1)
            {
                Sort();   
            }
            Save(names, sizes);

            Console.WriteLine("Successfully!");
        }
        else
        {
            goto again;
        }
        Console.ReadLine();
    }
    
    private static long CountFolderSize(string path)
    {
        if (!Directory.Exists(path))
        {
            throw new SystemException("No such folder!");
        }
        
        long size = 0;
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        try
        {
            FileInfo[] fileInfos = directoryInfo.GetFiles();

            foreach (var x in fileInfos)
            {
                size += x.Length;
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("unauthorized access exception! Skip");
        }

        // Count size of subdirectories
        try
        {
            DirectoryInfo[] subDirectoryInfos = directoryInfo.GetDirectories();
            foreach (var x in subDirectoryInfos)
            {
                size += CountFolderSize(x.FullName);
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("unauthorized access exception! Skip");
        }

        return size;
    }
    private static void Save(List<string> names, List<long> sizes)
    {
        if (!File.Exists("output.txt"))
        {
            File.Create("output.txt").Close(); 
        }
        
        // clear file
        File.WriteAllText("output.txt", String.Empty);
        
        for (int i = 0; i < names.Count; i++)
        {
            File.AppendAllText("output.txt", $"{names[i]}({sizes[i]/1048576}mb)\n");
        }
    }

    private static void Sort()
    {
        var resultSizes = new List<long>();
        var resultNames = new List<string>();
        var oldNames = names;
        var oldSizes = sizes;
        var oldNamesCount = oldNames.Count;
        var index = 0;
        for (int i = 0; i < oldNamesCount; i++)
        {
            index = oldSizes.IndexOf(oldSizes.Max());
            resultNames.Add(oldNames[index]);
            resultSizes.Add(oldSizes[index]);
            oldNames.RemoveAt(index);
            oldSizes.RemoveAt(index);
        }

        names = resultNames;
        sizes = resultSizes;
    }
}
