﻿using System.ComponentModel.DataAnnotations;

namespace ProjetoJqueryEstudos.Entities
{
    public class Address
    {       
        public int AddressId { get; set; }
        public long PersonId { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required, MinLength(8), MaxLength(8)]
        public string PostalCode { get; set; }
        [Required]
        public string Neighborhood { get; set; }
        [Required]
        public string PublicPlace { get; set; }
        public bool isPrincipalAddress { get; set; }

        public Address(long personId, string city, string state, string postalCode, string neighborhood, string publicPlace, bool isPrincipalAddress)
        {
            PersonId = personId;
            City = city;
            State = state;
            PostalCode = postalCode;
            Neighborhood = neighborhood;
            PublicPlace = publicPlace;
            this.isPrincipalAddress = isPrincipalAddress;
        }
    }
}
