# Dan.Plugin.Sjofart

Plugin for integrations towards Sjøfartsdirektoratet

## Adding mapping for new documents
When calling Sjøfartsdirektoratet's API for vessel data, it contains a collection of Document objects. These documents
do not share the same format, meaning they can't be reasonably deserialised into a simple POCO. To manage this, a
custom JsonConverter has been made [VesselDocumentConverter](src/Dan.Plugin.Sjofart/Converters/VesselDocumentConverter.cs).
Using the Document Classes found in the [VesselDocuments](src/Dan.Plugin.Sjofart/Models/VesselDocuments) folder,
this converter can determine what class to deserialise to as long as the Document Class has been implemented correctly.

Let's make an example class for a document:
```json
{
    "CertificateOfNationalityDate": "2012.03.20",
    "SrMeasurementData": {
        "ClassificationSociety": "Anonymised",
        "MeasurementCertificateCountry": "Anonymised",
        "VesselType": "STYKKGODSSKIP: - UBESTEMT",
        "BuildMaterial": "STÅL",
        "Propulsion": "MOTOR",
        "Length": {
            "Value": 81.07,
            "Unit": "METER"
        },
        "Width": {
            "Value": 12.8,
            "Unit": "METER"
        },
        "Depth": {
            "Value": 7.1,
            "Unit": "METER"
        },
        "GrossTonnage": 2528.0,
        "NetTonnage": 1372.0,
        "NISApprovedDate": "2012.02.16",
        "SeaworthyDate": "2012.02.16"
    },
    "Register": "Norsk Internasjonalt Skipsregister",
    "IsConferred": true,
    "DocumentTypeClass": "TE",
    "DocumentType": "MÅLEBREV",
    "JournalDate": "2012.03.20",
    "Date": "2000.06.22",
    "JournalKey": "Anonymised"
}
```

The identifiers for vessel documents are `"DocumentTypeClass"` and `"DocumentType"`. The class we make
needs to implement the interface `IDocumentIdentifiable` where we set these values. The class also needs to implement
`IVesselDocument`, the `VesselDocument` class is an abstract class that implements the interface that can be used too.

So with that we can make a class that looks like:
```csharp
[Serializable]
public class MeasurementDataDocument : VesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "TE";
    public static string[] DocumentTypeIdentifiers => ["MÅLEBREV"];
}
```

The DocumentTypeIdentifiers is an array, because the same document can sometimes have different DocumentTypes.
For Measurement Data Documents, `"MÅLEDATA"` is another value that is sometimes used, so we can add that to the array.

```csharp
[Serializable]
public class MeasurementDataDocument : VesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "TE";
    public static string[] DocumentTypeIdentifiers => ["MÅLEBREV", "MÅLEDATA"];
}
```

As long as those two static properties are set correctly, the rest can be implemented as a normally expected.
In this case we are interested in some of the data found in the SrMeasurementData property.

```csharp
[Serializable]
public class MeasurementDataDocument : VesselDocument, IDocumentIdentifiable
{
    public static string DocumentTypeClassIdentifier => "TE";
    public static string[] DocumentTypeIdentifiers => ["MÅLEBREV", "MÅLEDATA"];

    [JsonPropertyName("SrMeasurementData")]
    public SrMeasurementData SrMeasurementData { get; set; }
}
```

Finally, the [ResponseModelMapper](src/Dan.Plugin.Sjofart/Mappers/ResponseModelMapper.cs) must be updated
to map the data from the new document to the [ResponseModel](src/Dan.Plugin.Sjofart/Models/ResponseModel.cs) class.
