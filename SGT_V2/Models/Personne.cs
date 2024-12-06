using System;
using System.Collections.Generic;

namespace SGT_V2.Models;

public partial class Personne
{
    public int Idpersonne { get; set; }

    public int Matricule { get; set; }

    public string Nom { get; set; } = null!;

    public string Courriel { get; set; } = null!;

    public string Departement { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public override string ToString()
    {
        return Nom;
    }
}
