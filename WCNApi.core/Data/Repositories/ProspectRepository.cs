using Microsoft.Data.SqlClient;
using System.Data;
using WCNApi.core.Domain.DTOs;
using WCNApi.core.Domain.Models;
using WCNApi.core.Domain.Repositories;

namespace WCNApi.core.Data.Repositories
{
    public class ProspectRepository : IProspectRepository
    {
        private readonly AppDbContext _context;

        public ProspectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProspectDetailsDto?> GetProspectByIdAsync(int pId)
        {
            const string query = @"SELECT PId, NameofBusiness, DBA, Address, City, State, Zipcode,
                                    OwnerContact, Phone, Email, Industry, ProductType, Agency, Status, Active, CreatedDate,
                                    SalesRepId
                                FROM Tbl_Prospectdetails
                                WHERE PId = @PId
                                  AND Active = @Active";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@PId", pId),
                new SqlParameter("@Active", 1)
            };

            return await _context.ExecuteQuerySingleAsync<ProspectDetailsDto>(query, parameters);
        }
        public async Task<List<ProspectSearchResponse>>SearchProspectsAsync(ProspectSearchRequest request)
        {
            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@BusinessName", SqlDbType.NVarChar, 500)
                    {
                        Value = string.IsNullOrWhiteSpace(request.BusinessName)
                            ? DBNull.Value
                            : request.BusinessName.Trim()
                    },

                    new SqlParameter("@State", SqlDbType.NVarChar, 200)
                    {
                        Value = string.IsNullOrWhiteSpace(request.State)
                            ? DBNull.Value
                            : request.State.Trim()
                    },

                    new SqlParameter("@Status", SqlDbType.NVarChar, 200)
                    {
                        Value = string.IsNullOrWhiteSpace(request.Status)
                            ? DBNull.Value
                            : request.Status.Trim()
                    },

                    new SqlParameter("@Industry", SqlDbType.NVarChar, 100)
                    {
                        Value = string.IsNullOrWhiteSpace(request.Industry)
                            ? DBNull.Value
                            : request.Industry.Trim()
                    },

                    new SqlParameter("@SalesRepId", SqlDbType.Int)
                    {
                        Value = request.SalesRepId.HasValue
                            ? request.SalesRepId.Value
                            : DBNull.Value
                    },

                    new SqlParameter("@FromDate", SqlDbType.DateTime)
                    {
                        Value = request.FromDate.HasValue
                            ? request.FromDate.Value
                            : DBNull.Value
                    },

                    new SqlParameter("@ToDate", SqlDbType.DateTime)
                    {
                        Value = request.ToDate.HasValue
                            ? request.ToDate.Value.Date
                            : DBNull.Value
                    },

                    new SqlParameter("@Active", SqlDbType.Int)
                    {
                        Value = 1
                    
                    }
                };

            List<ProspectSearchResponse> result = await _context.ExecuteStoreProcedureAsync
                        <ProspectSearchResponse>(StoreProcedures.sp_SearchProspects_Dig, parameters);

            return result;
        }
    }
}
