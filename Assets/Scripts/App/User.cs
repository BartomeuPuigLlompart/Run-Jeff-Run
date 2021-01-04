using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string rol;
    public string correo;
    public string correoOpuesto;
    public string id;
    public string idOpuesto;
    public string usernameOpuesto;

    public User()
    {
    }
    public User(string _rol, string _correo, string _correoOpuesto, string _id, string _idOpuesto, string _usernameOpuesto = "Tutor")
    {
        rol = _rol;
        correo = _correo;
        correoOpuesto = _correoOpuesto;
        id = _id;
        idOpuesto = _idOpuesto;
        usernameOpuesto = _usernameOpuesto;
    }
}
