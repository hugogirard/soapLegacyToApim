using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class HL7Service : IHL7Service
{
    public string ProcessHl7Message(string message)
    {
        var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
        var resource = parser.Parse<Hl7.Fhir.Model.Resource>(message);

        // Process the HL7 message and generate a response
        var responseResource = ProcessHl7Resource(resource);

        // Serialize the response resource to a JSON string
        var serializer = new Hl7.Fhir.Serialization.FhirJsonSerializer();
        var jsonResponse = serializer.SerializeToString(responseResource);

        return jsonResponse;
    }

    private Hl7.Fhir.Model.Resource ProcessHl7Resource(Hl7.Fhir.Model.Resource resource)
    {
        if (resource.ResourceType == Hl7.Fhir.Model.ResourceType.Patient)
        {
            var patient = resource as Hl7.Fhir.Model.Patient;

            // Extract patient information from the HL7 message
            var patientName = patient.Name.FirstOrDefault();

            // Need to write exception here
            var firstName = patientName.Given.FirstOrDefault().ToString();
            var lastName = patient.Name.FirstOrDefault().Family.FirstOrDefault();

            // Perform some processing based on the extracted patient information
            var processedFirstName = !string.IsNullOrEmpty(firstName) ? firstName.ToUpper() : null;
            var processedLastName = !string.IsNullOrEmpty(lastName) ? lastName.ToUpper() : null;

            // Create a new patient resource for the response
            var responsePatient = new Hl7.Fhir.Model.Patient();

            // Update the patient resource with the processed information
            if (!string.IsNullOrEmpty(processedFirstName))
            {
                responsePatient.Name.Add(new Hl7.Fhir.Model.HumanName { Given = new List<string> { processedFirstName } });
            }
            if (!string.IsNullOrEmpty(processedLastName))
            {
                responsePatient.Name.First().Family = new List<string> { processedLastName };
            }

            // Optionally, you can save the processed patient resource to a database or perform other operations

            // Return the updated patient resource
            return responsePatient;
        }

        // If the resource type is not supported or recognized, you can throw an exception or return null
        throw new ArgumentException("Unsupported HL7 resource type.");
    }

}
