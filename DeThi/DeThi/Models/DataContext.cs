using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DeThi.Models
{
    public class CongNhanvaSoTrieuChung
    {
        private string hoten;
        private int namsinh;
        private string nuocve;
        private int sotrieuchung;
        public string HoTen { get => this.hoten; set => this.hoten = value; }
        public int NamSinh { get => this.namsinh; set => this.namsinh = value; }
        public string NuocVe { get => this.nuocve; set => this.nuocve = value; }
        public int SoTrieuChung { get => this.sotrieuchung; set => this.sotrieuchung = value; }
    }
    public class DataContext
    {
        public string ConnectionString { get; set; }

        public DataContext(string connectionstring)
        {
            this.ConnectionString = connectionstring;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        public bool InsertDiemCachLy(DiemCachLyModels diemCachLy)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                var query = "INSERT INTO DiemCachLy VALUES (@MaDiemCachLy, @TenDiemCachLy, @DiaChi)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("MaDiemCachLy", diemCachLy.MaDiemCachLy);
                cmd.Parameters.AddWithValue("TenDiemCachLy", diemCachLy.TenDiemCachLy);
                cmd.Parameters.AddWithValue("DiaChi", diemCachLy.DiaChi);

                var result = cmd.ExecuteNonQuery() != 0;

                conn.Close();

                return result;
            }
        }
        public List<CongNhanvaSoTrieuChung> sqlListBySoTC(int SoTrieuChung)
        {
            List<CongNhanvaSoTrieuChung> list = new List<CongNhanvaSoTrieuChung>();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "Select TENCONGNHAN, NAMSINH, NUOCVE, Count(*) as sotrieuchung " +
                    "From CONGNHAN cn join CN_TC n on cn.MACONGNHAN = n.MACONGNHAN " +
                    "Group By cn.TENCONGNHAN, NAMSINH,NUOCVE " +
                    "Having COUNT(*) >= @sotrieuchung";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("sotrieuchung", SoTrieuChung);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CongNhanvaSoTrieuChung()
                        {
                            HoTen = reader["tencongnhan"].ToString(),
                            NamSinh = Convert.ToInt32(reader["namsinh"].ToString()),
                            NuocVe = reader["nuocve"].ToString(),
                            SoTrieuChung = Convert.ToInt32(reader["sotrieuchung"].ToString()),
                        });
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return list;
        }
        public List<DiemCachLyModels> sqlListDiemCachLy()
        {
            List<DiemCachLyModels> list = new List<DiemCachLyModels>();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "Select * From DiemCachLy";
                SqlCommand cmd = new SqlCommand(query, conn);
                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DiemCachLyModels()
                        {
                            MaDiemCachLy = reader["madiemcachly"].ToString(),
                            TenDiemCachLy = reader["tendiemcachly"].ToString(),
                            DiaChi = reader["diachi"].ToString(),
                        });
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return list;
        }

        public List<CongNhanModels> sqlListCongNhanCachLy(string madiemcachly)
        {
            List<CongNhanModels> list = new List<CongNhanModels>();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string query = "Select * From CONGNHAN Where MaDiemCachLy = @MaDiemCachLy";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("MaDiemCachLy", madiemcachly);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new CongNhanModels()
                        {
                            MaCongNhan = reader["macongnhan"].ToString(),
                            TenCongNhan = reader["tencongnhan"].ToString(),
                        });
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return list;
        }
        public bool DeleteCongNhan(string maCongNhan)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                var query = @"DELETE FROM CN_TC WHERE MaCongNhan = @MaCongNhan;
                            DELETE FROM CongNhan WHERE MaCongNhan = @MaCongNhan";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("MaCongNhan", maCongNhan);

                var result = cmd.ExecuteNonQuery() != 0;

                conn.Close();

                return result;
            }
        }
        public CongNhanModels GetCongNhan(string maCongNhan)
        {
            CongNhanModels ketqua = new CongNhanModels();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                var query = @"SELECT * FROM CongNhan WHERE MaCongNhan = @MaCongNhan;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("MaCongNhan", maCongNhan);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();

                    ketqua.MaCongNhan = reader.GetString(0);
                    ketqua.TenCongNhan = reader.GetString(1);
                    ketqua.GioiTinh = reader.GetBoolean(2);
                    ketqua.NamSinh = reader.GetInt32(3);
                    ketqua.NuocVe = reader.GetString(4);
                    ketqua.MaDiemCachLy = reader.GetString(5);

                    reader.Close();
                }
                conn.Close();
            }
            return ketqua;
        }
    }
}
