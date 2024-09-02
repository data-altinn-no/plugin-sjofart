using System;
using System.Linq;
using Dan.Plugin.Sjofart.Models;
using Dan.Plugin.Sjofart.Models.VesselDocuments;
using Dan.Plugin.Sjofart.Models.VesselDocuments.DocumentProperties;
using Newtonsoft.Json;

namespace Dan.Plugin.Sjofart.Test.Converters;

public class VesselDocumentConverterTests
{
    [Fact]
    public void Deserialize_VesselDataHasHjemmelsDokument_ShouldGetAuthorityDocument()
    {
        // Arrange
        const string json = """
        {
            "Documents":[
               {
                   "DocumentTypeClass": "HJ",
                   "DocumentType": "HJEMMELSDOKUMENT",
                   "Date": "2024.08.29",
                   "Roles": [
                       {
                           "RoleType": "RoleType",
                           "LegalEntity": {
                               "Name": "Name",
                               "EntityId": "EntityId"
                           }
                       }
                   ]
               }
           ]
        }
        """;

        var expected = new AuthorityDocument
        {
            Date = new DateTime(2024, 8, 29),
            Roles =
            [
                new DocumentRole
                {
                    RoleType = "RoleType",
                    LegalEntity = new LegalEntity
                    {
                        Name = "Name",
                        EntityId = "EntityId"
                    }
                }
            ]
        };

        // Act
        var historicalVesselData = JsonConvert.DeserializeObject<HistoricalVesselData>(json);
        var actual = historicalVesselData?.Documents?.FirstOrDefault();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_VesselDataHasSkjoeteDokument_ShouldGetAuthorityDocument()
    {
        // Arrange
        const string json = """
        {
            "Documents":[
               {
                   "DocumentTypeClass": "HJ",
                   "DocumentType": "SKJØTE",
                   "Date": "2024.08.29",
                   "Roles": [
                       {
                           "RoleType": "RoleType",
                           "LegalEntity": {
                               "Name": "Name",
                               "EntityId": "EntityId"
                           }
                       }
                   ]
               }
           ]
        }
        """;

        var expected = new AuthorityDocument
        {
            Date = new DateTime(2024, 8, 29),
            Roles =
            [
                new DocumentRole
                {
                    RoleType = "RoleType",
                    LegalEntity = new LegalEntity
                    {
                        Name = "Name",
                        EntityId = "EntityId"
                    }
                }
            ]
        };

        // Act
        var historicalVesselData = JsonConvert.DeserializeObject<HistoricalVesselData>(json);
        var actual = historicalVesselData?.Documents?.FirstOrDefault();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_VesselDataHasNorDriftDocument_ShouldGetMaintenanceDocument()
    {
        // Arrange
        const string json = """
        {
            "Documents":[
               {
                   "DocumentTypeClass": "HJ",
                   "DocumentType": "DRIFT IHT SKIPSSIKKERHETSLOVEN",
                   "Date": "2024.08.29",
                   "Roles": [
                       {
                           "RoleType": "RoleType",
                           "LegalEntity": {
                               "Name": "Name",
                               "EntityId": "EntityId"
                           }
                       }
                   ]
               }
           ]
        }
        """;

        var expected = new MaintenanceDocument
        {
            Date = new DateTime(2024, 8, 29),
            Roles =
            [
                new DocumentRole
                {
                    RoleType = "RoleType",
                    LegalEntity = new LegalEntity
                    {
                        Name = "Name",
                        EntityId = "EntityId"
                    }
                }
            ]
        };

        // Act
        var historicalVesselData = JsonConvert.DeserializeObject<HistoricalVesselData>(json);
        var actual = historicalVesselData?.Documents?.FirstOrDefault();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_VesselDataHasNisDriftDocument_ShouldGetMaintenanceDocument()
    {
        // Arrange
        const string json = """
        {
            "Documents":[
               {
                   "DocumentTypeClass": "HJ",
                   "DocumentType": "DRIFT IHHT SJØLOVEN/NIS LOVEN",
                   "Date": "2024.08.29",
                   "Roles": [
                       {
                           "RoleType": "RoleType",
                           "LegalEntity": {
                               "Name": "Name",
                               "EntityId": "EntityId"
                           }
                       }
                   ]
               }
           ]
        }
        """;

        var expected = new MaintenanceDocument
        {
            Date = new DateTime(2024, 8, 29),
            Roles =
            [
                new DocumentRole
                {
                    RoleType = "RoleType",
                    LegalEntity = new LegalEntity
                    {
                        Name = "Name",
                        EntityId = "EntityId"
                    }
                }
            ]
        };

        // Act
        var historicalVesselData = JsonConvert.DeserializeObject<HistoricalVesselData>(json);
        var actual = historicalVesselData?.Documents?.FirstOrDefault();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_VesselDataHasMaalebrevDocument_ShouldGetMeasurementDataDocument()
    {
        // Arrange
        const string json = """
        {
            "Documents":[
               {
                   "DocumentTypeClass": "TE",
                   "DocumentType": "MÅLEBREV",
                   "Date": "2024.08.29",
                   "SrMeasurementData": {
                        "VesselType": "VesselType",
                        "GrossTonnage": 2.50,
                        "NetTonnage": 1.50,
                    }
               }
           ]
        }
        """;

        var expected = new MeasurementDataDocument
        {
            Date = new DateTime(2024, 8, 29),
            SrMeasurementData = new SrMeasurementData
            {
                VesselType = "VesselType",
                GrossTonnage = 2.5,
                NetTonnage = 1.5
            }
        };

        // Act
        var historicalVesselData = JsonConvert.DeserializeObject<HistoricalVesselData>(json);
        var actual = historicalVesselData?.Documents?.FirstOrDefault();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_VesselDataHasMaaledataDocument_ShouldGetMeasurementDataDocument()
    {
        // Arrange
        const string json = """
        {
            "Documents":[
               {
                   "DocumentTypeClass": "TE",
                   "DocumentType": "MÅLEDATA",
                   "Date": "2024.08.29",
                   "SrMeasurementData": {
                       "VesselType": "VesselType",
                       "GrossTonnage": 2.50,
                       "NetTonnage": 1.50,
                    }
               }
           ]
        }
        """;

        var expected = new MeasurementDataDocument
        {
            Date = new DateTime(2024, 8, 29),
            SrMeasurementData = new SrMeasurementData
            {
                VesselType = "VesselType",
                GrossTonnage = 2.5,
                NetTonnage = 1.5
            }
        };

        // Act
        var historicalVesselData = JsonConvert.DeserializeObject<HistoricalVesselData>(json);
        var actual = historicalVesselData?.Documents?.FirstOrDefault();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_VesselDataHasMessageDocument_ShouldGetMessageDocument()
    {
        // Arrange
        const string json = """
        {
            "Documents":[
               {
                   "DocumentTypeClass": "ET",
                   "DocumentType": "MELDING",
                   "Date": "2024.08.29",
                   "Construction": {
                       "Year": 2024
                    }
               }
           ]
        }
        """;

        var expected = new MessageDocument
        {
            Date = new DateTime(2024, 8, 29),
            Construction = new Construction
            {
                Year = 2024
            }
        };

        // Act
        var historicalVesselData = JsonConvert.DeserializeObject<HistoricalVesselData>(json);
        var actual = historicalVesselData?.Documents?.FirstOrDefault();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Deserialize_VesselDataHasMessageShipyardDocument_ShouldGetShipyardDocument()
    {
        // Arrange
        const string json = """
        {
            "Documents":[
               {
                   "DocumentTypeClass": "ET",
                   "DocumentType": "MELDING BYGGEVERFT",
                   "Date": "2024.08.29",
                   "Construction": {
                       "Shipyard": "Shipyard"
                    }
               }
           ]
        }
        """;

        var expected = new ShipyardDocument
        {
            Date = new DateTime(2024, 8, 29),
            Construction = new Construction
            {
                Shipyard = "Shipyard"
            }
        };

        // Act
        var historicalVesselData = JsonConvert.DeserializeObject<HistoricalVesselData>(json);
        var actual = historicalVesselData?.Documents?.FirstOrDefault();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
