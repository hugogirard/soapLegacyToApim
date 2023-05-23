using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{

	[OperationContract]
    PatientInfo ExtractPatientName(string message);
	
}

[DataContract]
public class PatientInfo
{
    string _lastName;
    string _firstName;

	[DataMember]
	public string LastName
	{
		get { return _lastName; }
		set { _lastName = value; }
	}

	[DataMember]
	public string FirstName
	{
		get { return _firstName; }
		set { _firstName = value; }
	}
}
