using System;
using System.Collections.Generic;

namespace SGT_V2.Models;

public partial class Commentaire
{
    public int Idcommentaire { get; set; }

    public int IdticketCommentaire { get; set; }

    public DateTime Datecommentaire { get; set; }

    public string Contenu { get; set; } = null!;

    public virtual Ticket IdticketCommentaireNavigation { get; set; } = null!;
}
