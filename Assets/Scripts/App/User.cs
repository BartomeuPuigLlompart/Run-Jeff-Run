using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string rol;
    public string correo;
    public string id;
    public string idOpuesto;

    public User()
    {
    }
    public User(string _rol, string _correo, string _id, string _idOpuesto)
    {
        rol = _rol;
        correo = _correo;
        id = _id;
        idOpuesto = _idOpuesto;
    }
}
