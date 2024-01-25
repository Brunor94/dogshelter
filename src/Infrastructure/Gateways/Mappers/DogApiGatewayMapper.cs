namespace DogShelterService.Infrastructure.Gateways.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Domain.Enums;
    using DogShelterService.Infrastructure.Gateways.Mappers.Interfaces;
    using DogShelterService.Infrastructure.Gateways.Models;

    public sealed class DogApiGatewayMapper : IDogApiGatewayMapper
    {
        public Breed Map(DogApiGatewayDTO dogApiGatewayDTO)
        {
            if (dogApiGatewayDTO is null)
            {
                return null;
            }

            var avgHeight = this.CalculateAvgHeightFromHeightRangeMetric(dogApiGatewayDTO.Height.Metric);

            return new Breed
            {
                Name = dogApiGatewayDTO.Name,
                Temperaments = dogApiGatewayDTO.Temperament?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList() ?? new List<string>(),
                SizeCategory = ConvertHeightSizeToCategory(avgHeight),
                AvgHeight = avgHeight
            };
        }

        private SizeCategory ConvertHeightSizeToCategory(double avgHeight)
        {
            if (avgHeight < 35)
            {
                return SizeCategory.Small;
            }
            else if (avgHeight >= 35 && avgHeight <= 55)
            {
                return SizeCategory.Medium;
            }

            return SizeCategory.Large;
        }

        private double CalculateAvgHeightFromHeightRangeMetric(string height)
        {
            var splitHeight = height?.Split('-');
            if (splitHeight is null || splitHeight.Length != 2)
            {
                throw new ArgumentException("Height must be in the format 'lower - upper'");
            }

            if (!int.TryParse(splitHeight[0].Trim(), out var lowerBound) ||
                !int.TryParse(splitHeight[1].Trim(), out var upperBound))
            {
                throw new ArgumentException("Height values must be integers.");
            }

            return (lowerBound + upperBound) / 2;
        }
    }
}