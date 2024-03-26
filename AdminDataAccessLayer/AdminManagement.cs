using System.Configuration;
using System.Data.SqlClient;

namespace AdminDataAccessLayer
{
    public class AdminManagement
    {
        public string kitchenStory;
        public SqlConnection con;
        public AdminManagement()
        {
            kitchenStory = ConfigurationManager.ConnectionStrings["KitchenDB"].ConnectionString;
            con = new SqlConnection(kitchenStory);
        }

        public bool AdminLogin(AdminDTO adminDTO)
        {
            SqlCommand cmd = new SqlCommand("Select Password from Admin where EmailId = @EmailId", con);
            cmd.Parameters.AddWithValue("@EmailId", adminDTO.EmailId);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string strPassword = reader["Password"].ToString();
                if (strPassword == adminDTO.Password)
                {
                    return true;
                }
            }
            return false;
        }


        public bool ChangePassword(AdminDTO adminDTO)
        {
            SqlCommand cmd = new SqlCommand("dbo.sp_updatePassword", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_EmailId", adminDTO.EmailId);
            cmd.Parameters.AddWithValue("@p_Password", adminDTO.Password);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            return true;
        }

        public bool validEmail(string EmailId)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Admin WHERE EmailId = @EmailId", con);
            cmd.Parameters.AddWithValue("@EmailId", EmailId);

            con.Open();
            int count = (int)cmd.ExecuteScalar();
            con.Close();

            return count > 0;
        }
    }
}
