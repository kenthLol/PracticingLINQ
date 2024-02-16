using PracticingLINQ;

foreach(var student in Classroom.students)
{
    Console.WriteLine($"Nombre: {student.Firstname} \nApellido: {student.Lastname}\n");
    foreach(var score in student.Scores!)
    {
        Console.WriteLine($"Calificación: {score}");
    }
    Console.WriteLine("");
}

/*
 *   1. Obtener el origen de la consulta
 *   2. Crear la consulta
 *   3. Ejecutar la consulta
 *   
 *   La variable de consulta es de tipo IEnumerable<Student>, que es una colección de objetos Student.
 *   La consulta utiliza la cláusula from para especificar la colección que se consultará.
 *   La cláusula donde filtra la colección según una condición.
 *   La cláusula de selección especifica el tipo de elementos devueltos.
 */


var studentQuery = 
    from student in Classroom.students
    where student.Scores![0] > 90 && student.Scores![3] < 80
    select student;

//Muestra los estudiantes con una primera calificación superior a 90

Console.WriteLine("Students with a first score over 90:\n");
foreach(Student student in studentQuery)
{
    Console.WriteLine("{0}, {1}", student.Lastname, student.Firstname);
}

/*
 * Se obtiene los estudiantes cuya primera nota está por encima de los 90
 * y su última nota por debajo de los 80. Luego se ordenan los estudiantes
 * según su apellido en forma ascendente
 */

Console.WriteLine("\nStudents sorted by last name in ascending order:\n");

var orderedStudentQuery =
    from student in Classroom.students
    where student.Scores![0] > 90 && student.Scores![3] < 80
    orderby student.Lastname ascending
    select student;

foreach(Student student in orderedStudentQuery)
{
    Console.WriteLine("{0}, {1}", student.Lastname, student.Firstname);
}

Student newStudent = new Student
{
    Firstname = "Kenneth",
    Lastname = "Lola",
    Id = 119,
    Scores = new List<int> { 90, 90, 90, 100 }
};

Classroom.students.Add(newStudent);

/*
 * Se obtiene los estudiantes cuya primera nota está por encima de los 90
 * y su última nota por debajo de los 80. Luego se ordenan los estudiantes
 * según su primera nota en orden descendente
 */

Console.WriteLine("\nSort the students in descending order according to their first score:\n");

var orderedStudentQuery2 =
    from student in Classroom.students
    where student.Scores![0] > 90 && student.Scores![3] < 80
    orderby student.Scores![0] descending
    select student;

foreach(Student student in orderedStudentQuery2)
{
    Console.WriteLine("{0}, {1}, Score: {2}", student.Lastname, student.Firstname, student.Scores![0]);
}

Console.WriteLine("");

/*
 * Genera una secuencia de objetos de grupo donde la clave (tipo char) 
 * es el primer carácter del apellido y una secuencia de objetos Student
 */
Console.WriteLine("\nGroup students by first letter of last name:\n");

IEnumerable<IGrouping<char, Student>> studentQuery3 =
    from student in Classroom.students
    // orderby student.Lastname![0] ascending - Forma alternativa
    group student by student.Lastname![0] into studentGroup
    orderby studentGroup.Key //Ordena los grupos por la clave
    select studentGroup;

// studentGroup es un identificador que representa cada grupo de estudiantes

foreach(IGrouping<char, Student> studentGroup in studentQuery3)
{
    Console.WriteLine(studentGroup.Key);
    foreach(Student student in studentGroup)
    {
        Console.WriteLine("   {0}, {1}", student.Lastname, student.Firstname);
    }
}


/*
 * studentQuery4 es un IEnumerable<string>
 * Esta consulta retorna esos estudiantes cuya primera
 * nota es mayor que el promedio de sus notas
 */

Console.WriteLine("\nShows students whose first grade is higher than their average:\n");
var studentQuery4 = 
    from student in Classroom.students
    let totalScore = student.Scores![0] + student.Scores[1] + student.Scores[2] + student.Scores[3]
    where totalScore / 4 < student.Scores![0]
    select student.Lastname + " " + student.Firstname;
 
foreach (string student in studentQuery4)
{
    Console.WriteLine(student);
}

Console.WriteLine("");

/*
 * Otra manera
 */

Console.WriteLine("\nShows students whose first grade is higher than their average:\n");
var studentQuery5 =
    from student in Classroom.students
    let totalScore = student.Scores![0] + student.Scores[1] + student.Scores[2] + student.Scores[3]
    select totalScore;

double averageScore = studentQuery5.Average();

var studentQuery6 =
    from student in Classroom.students
    let scores = student.Scores![0] + student.Scores[1] + student.Scores[2] + student.Scores[3]
    where scores > averageScore
    select new { id = student.Id, score = scores };

foreach (var student in studentQuery6)
{
    Console.WriteLine($"Student ID: {student.id}, Score: {student.score}");
}
