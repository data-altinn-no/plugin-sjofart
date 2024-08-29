using System;
using Dan.Plugin.Sjofart.Mappers;
using Dan.Plugin.Sjofart.Models;
using Dan.Plugin.Sjofart.Models.VesselDocuments;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;

namespace Dan.Plugin.Sjofart.Test.Mappers;

public class ResponseModelMapperTests
{
    [Fact]
    public void Map_RootData_ShouldMap()
    {
        // Arrange
        var mapper = new ResponseModelMapper();
        var input = new HistoricalVesselData
        {
            VesselId = 1,
            CallSign = "CallSign",
            Register = "Register",
            Imo = 2,
            Status = "Status",
            Name = "Name"
        };

        var expected = new ResponseModel
        {
            VesselId = 1,
            CallSign = "CallSign",
            Registry = "Register",
            Imo = "2",
            Status = "Status",
            ShipName = "Name"
        };

        // Act
        var actual = mapper.Map(input);

        // Asset
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Map_MeasurementDocumentFound_ShouldGetMeasurementData()
    {
        // Arrange
        var mapper = new ResponseModelMapper();
        var input = new HistoricalVesselData
        {
            Documents =
            [
                new MeasurementDataDocument
                {
                    SrMeasurementData = new SrMeasurementData()
                    {
                      GrossTonnage  = 1.5,
                      NetTonnage = 2.5,
                      VesselType = "VesselType"
                    },
                    Date = new DateTime(2024, 8, 29)
                }
            ]
        };

        var expected = new ResponseModel
        {
            ShipType = "VesselType",
            GrossTonnage = 1.5,
            NetTonnage = 2.5
        };

        // Act
        var actual = mapper.Map(input);

        // Asset
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Map_OwnerDocumentFound_ShouldGetOwnerData()
    {
        // Arrange
        var mapper = new ResponseModelMapper();
        var input = new HistoricalVesselData
        {
            Documents =
            [
                new AuthorityDocument()
                {
                    Roles =
                    [
                        new DocumentRole
                        {
                            LegalEntity = new LegalEntity
                            {
                                EntityId = "EntityId",
                                Name = "Name"
                            },
                            RoleType = "eier"
                        }
                    ],
                    Date = new DateTime(2024, 8, 29)
                }
            ]
        };

        var expected = new ResponseModel
        {
            OwnerName = "Name",
            OwnerOrgNumber = "EntityId"
        };

        // Act
        var actual = mapper.Map(input);

        // Asset
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Map_MaintenanceDocumentFound_ShouldGetMaintenanceCompanyData()
    {
        // Arrange
        var mapper = new ResponseModelMapper();
        var input = new HistoricalVesselData
        {
            Documents =
            [
                new MaintenanceDocument()
                {
                    Roles =
                    [
                        new DocumentRole
                        {
                            LegalEntity = new LegalEntity
                            {
                                EntityId = "EntityId",
                                Name = "Name"
                            },
                            RoleType = "driftsselskap"
                        }
                    ],
                    Date = new DateTime(2024, 8, 29)
                }
            ]
        };

        var expected = new ResponseModel
        {
            OperatingOrganisationName = "Name",
            OperationOrganisationNo = "EntityId"
        };

        // Act
        var actual = mapper.Map(input);

        // Asset
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Map_MessageDocumentFound_ShouldGetYearBuilt()
    {
        // Arrange
        var mapper = new ResponseModelMapper();
        var input = new HistoricalVesselData
        {
            Documents =
            [
                new MessageDocument()
                {
                    Construction = new Construction
                    {
                        Year = 2024
                    },
                    Date = new DateTime(2024, 8, 29)
                }
            ]
        };

        var expected = new ResponseModel
        {
            YearBuilt = 2024
        };

        // Act
        var actual = mapper.Map(input);

        // Asset
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Map_ShipyardDocumentFound_ShouldGetShipyard()
    {
        // Arrange
        var mapper = new ResponseModelMapper();
        var input = new HistoricalVesselData
        {
            Documents =
            [
                new ShipyardDocument()
                {
                    Construction = new Construction
                    {
                        Shipyard = "Shipyard"
                    },
                    Date = new DateTime(2024, 8, 29)
                }
            ]
        };

        var expected = new ResponseModel
        {
            ShipYard = "Shipyard"
        };

        // Act
        var actual = mapper.Map(input);

        // Asset
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Map_MultipleMeasurementDocumentsFound_ShouldUseMostRecentData()
    {
        // Arrange
        var mapper = new ResponseModelMapper();
        var input = new HistoricalVesselData
        {
            Documents =
            [
                new MeasurementDataDocument
                {
                    SrMeasurementData = new SrMeasurementData()
                    {
                        GrossTonnage  = 1.5,
                        NetTonnage = 2.5,
                        VesselType = "OldVesselType"
                    },
                    Date = new DateTime(1990, 8, 29)
                },
                new MeasurementDataDocument
                {
                    SrMeasurementData = new SrMeasurementData()
                    {
                        GrossTonnage  = 5.4,
                        NetTonnage = 3.2,
                        VesselType = "NewVesselType"
                    },
                    Date = new DateTime(2024, 8, 29)
                }
            ]
        };

        var expected = new ResponseModel
        {
            GrossTonnage = 5.4,
            NetTonnage = 3.2,
            ShipType = "NewVesselType"
        };

        // Act
        var actual = mapper.Map(input);

        // Asset
        actual.Should().BeEquivalentTo(expected);
    }
}
