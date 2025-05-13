using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Student", menuName = "ScriptableObjects/Example/Student")]
public class StudentSO : ScriptableObject
{
    [SerializeField] private Student studentData;

    public Student StudentData => studentData;


    public Student GetBasicStudentData()
    {
        return new Student("Alumno", "Nickname", 18, 70001225, "Jueguitos");
    }
}
