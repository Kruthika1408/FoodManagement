using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace FoodDataAccessLayer
{
    public class FoodManagement
    {
        public string kitchenStory;
        public SqlConnection con;

        public FoodManagement()
        {
            kitchenStory = ConfigurationManager.ConnectionStrings["KitchenDB"].ConnectionString;
            con = new SqlConnection(kitchenStory);
        }

        public List<FoodDTO> GetAllFoodItem()
        {
            List<FoodDTO> foodItemList = new List<FoodDTO>();
            SqlCommand cmd = new SqlCommand("Select * from FoodItems", con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                FoodDTO item = new FoodDTO();
                item.Id = Convert.ToInt32(reader["Id"]);
                item.FoodName = reader["FName"].ToString();
                item.Price = Convert.ToSingle(reader["FPrice"]);
                foodItemList.Add(item);
            }
            con.Close();
            con.Dispose();
            return foodItemList;
        }

        public FoodDTO GetFoodItemById(int id)
        {
            SqlCommand cmd = new SqlCommand("Select * from FoodItems where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            FoodDTO item = null;

            if (reader.HasRows)
            {
                reader.Read();

                item = new FoodDTO
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    FoodName = reader["FName"].ToString(),
                    Price = Convert.ToSingle(reader["FPrice"])
                };
            }

            con.Close();
            con.Dispose();
            return item;
        }


        public bool AddFoodItem(FoodDTO foodMaster)
        {
            SqlCommand cmd = new SqlCommand("dbo.sp_InsertFoodItems", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_FName", foodMaster.FoodName);
            cmd.Parameters.AddWithValue("@p_FPrice", foodMaster.Price);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            return true;
        }

        public bool UpdateFoodItem(FoodDTO foodMaster)
        {
            SqlCommand cmd = new SqlCommand("dbo.sp_updateFoodItems", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_Id", foodMaster.Id);
            cmd.Parameters.AddWithValue("@p_FName", foodMaster.FoodName);
            cmd.Parameters.AddWithValue("@p_FPrice", foodMaster.Price);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            return true;

        }
        public bool DeleteFoodItem(int id)
        {
            SqlCommand cmd = new SqlCommand("Delete * from FoodItems where Id=" + id, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            return true;
        }
    }
}
