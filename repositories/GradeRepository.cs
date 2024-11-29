```csharp
using System;
using System.Collections.Generic;
using System.IO;

namespace Repositories
{
    public interface IGradeRepository
    {
        List<Grade> GetGrades();
    }

    public class GradeRepository : IGradeRepository
    {
        private readonly string _dataSource = "grades.txt";

        public List<Grade> GetGrades()
        {
            try
            {
                if (!File.Exists(_dataSource))
                {
                    throw new FileNotFoundException("The data source file does not exist.", _dataSource);
                }

                var grades = new List<Grade>();

                using (var reader = new StreamReader(_dataSource))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            if (int.TryParse(parts[0], out int id) &&
                                decimal.TryParse(parts[2], out decimal value))
                            {
                                grades.Add(new Grade { Id = id, Name = parts[1], Value = value });
                            }
                            else
                            {
                                throw new FormatException("Invalid data format in the data source file.");
                            }
                        }
                        else
                        {
                            throw new FormatException("Invalid data format in the data source file.");
                        }
                    }
                }

                return grades;
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception($"The data source file {_dataSource} does not exist.", ex);
            }
            catch (FormatException ex)
            {
                throw new Exception("Invalid data format in the data source file.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving grades.", ex);
            }
        }
    }

    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
```