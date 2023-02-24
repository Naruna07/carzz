using System;
using System.ComponentModel.DataAnnotations;

namespace JudgeMyTaste.Models
{
    public class FavoriteBand
    {
        public int Id { get; set; }

        [Display(Name="Band/Artist Name")]
        public string Name { get; set; }

        [Display(Name="Entered By")]
        public string EnteredBy { get; set; }

        [Display(Name="Entered On")]
        public DateTime EnteredOn { get; set; }
    }
}