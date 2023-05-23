using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    public PatientInfo ExtractPatientName(string message)
    {
        // Split the HL7 message into segments
        var segments = message.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        
        // Find the PID segment
        var pidSegment = segments.FirstOrDefault(s => s.StartsWith("PID"));

        if (pidSegment != null)
        {
            // Split the PID segment into fields
            var fields = pidSegment.Split('|');

            // Get the patient's name field (index 5)
            var nameField = fields.ElementAtOrDefault(5);

            if (!string.IsNullOrEmpty(nameField))
            {
                // Split the patient's name field into components
                var nameComponents = nameField.Split('^');

                if (nameComponents.Length >= 2)
                {
                    // Extract the first name (index 1) and last name (index 0)
                    var firstName = nameComponents[1];
                    var lastName = nameComponents[0];
                    
                    PatientInfo patientInfo = new PatientInfo 
                    {
                        FirstName = firstName,
                        LastName = lastName,
                    };

                    return patientInfo;
                }
            }
        }
        return null;
    }
}
