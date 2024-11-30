using System;
using System.Collections.Generic;

namespace SGT_V2.Models;

public partial class Ticket
{
    public int Idticket { get; set; }

    public int IdpersonneTickets { get; set; }

    public string Titre { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Categorie { get; set; } = null!;

    public string Priorite { get; set; } = null!;

    public DateTime Datecreation { get; set; }

    public DateTime? Datefermeture { get; set; }

    public virtual ICollection<Commentaire> Commentaires { get; set; } = new List<Commentaire>();

    public virtual Personne IdpersonneTicketsNavigation { get; set; } = null!;
}
