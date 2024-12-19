namespace PRE_DEFENSE
{
    internal class EditPatientForm
    {
        private string patientID;
        private string patientName;
        private string patientLastName;

        public EditPatientForm(string patientID, string patientName, string patientLastName)
        {
            this.patientID = patientID;
            this.patientName = patientName;
            this.patientLastName = patientLastName;
        }
    }
}