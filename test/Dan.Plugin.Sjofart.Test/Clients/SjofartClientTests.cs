using System.Collections.Generic;
using Dan.Plugin.Sjofart.Clients;
using Dan.Plugin.Sjofart.Models;
using Dan.Plugin.Sjofart.Models.VesselDocuments;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Test.Clients;

public class SjofartClientTests
{
    [Fact]
    public void GetRelevantVessels_HalfRelevantVessels_ShouldOnlyGetRelevantVessels()
    {
        // Arrange
        const string legalEntityId = "relevantLegalEntity";
        var vessels = new List<HistoricalVesselData>
        {
            new()
            {
                VesselId = 1,
                Documents =
                [
                    new AuthorityDocument
                    {
                        Roles = [
                            new DocumentRole
                            {
                                RoleType = "eier",
                                LegalEntity = new LegalEntity
                                {
                                    EntityId = "relevantLegalEntity",
                                    Name = "relevantName"
                                }
                            }
                        ]
                    }
                ]
            },
            new()
            {
                VesselId = 2,
                Documents =
                [
                    new AuthorityDocument
                    {
                        Roles = [
                            new DocumentRole
                            {
                                RoleType = "eier",
                                LegalEntity = new LegalEntity
                                {
                                    EntityId = "irrelevantLegalEntity",
                                    Name = "irrelevantName"
                                }
                            }
                        ]
                    }
                ]
            },
            new()
            {
                VesselId = 3,
                Documents =
                [
                    new AuthorityDocument
                    {
                        Roles = [
                            new DocumentRole
                            {
                                RoleType = "driftsansvarlig",
                                LegalEntity = new LegalEntity
                                {
                                    EntityId = "relevantLegalEntity",
                                    Name = "relevantName"
                                }
                            }
                        ]
                    }
                ]
            },
            new()
            {
                VesselId = 4,
                Documents =
                [
                    new AuthorityDocument
                    {
                        Roles = [
                            new DocumentRole
                            {
                                RoleType = "driftsansvarlig",
                                LegalEntity = new LegalEntity
                                {
                                    EntityId = "irrelevantLegalEntity",
                                    Name = "irrelevantName"
                                }
                            }
                        ]
                    }
                ]
            }
        };

        var expected = new List<HistoricalVesselData>
        {
            new()
            {
                VesselId = 1,
                Documents =
                [
                    new AuthorityDocument
                    {
                        Roles = [
                            new DocumentRole
                            {
                                RoleType = "eier",
                                LegalEntity = new LegalEntity
                                {
                                    EntityId = "relevantLegalEntity",
                                    Name = "relevantName"
                                }
                            }
                        ]
                    }
                ]
            },
            new()
            {
                VesselId = 3,
                Documents =
                [
                    new AuthorityDocument
                    {
                        Roles = [
                            new DocumentRole
                            {
                                RoleType = "driftsansvarlig",
                                LegalEntity = new LegalEntity
                                {
                                    EntityId = "relevantLegalEntity",
                                    Name = "relevantName"
                                }
                            }
                        ]
                    }
                ]
            }
        };

        // Act
        var actual = SjofartClient.GetRelevantVessels(vessels, legalEntityId);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
