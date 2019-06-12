using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.IdentityExtensions.Components;
using MRApiCommon.Infrastructure.IdentityExtensions.Interface;
using MRApiCommon.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MRApiCommon.Infrastructure.IdentityExtensions.Store
{
    public class MRRoleStore : MRMongoRepository<MRRole>, IMRRoleStore
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings"></param>
        public MRRoleStore(IOptions<MRDbOptions> settings) : base(settings) { }

        public async Task<IdentityResult> CreateAsync(MRRole role, CancellationToken cancellationToken)
        {
            var exists = await Any(x => x.NormalizedName == role.NormalizedName);
            if (exists)
            {
                return IdentityResult.Failed(new IdentityError[] { new IdentityError
                {
                    Code = "0",
                    Description = "Role with this name already exists"
                } });
            }

            await Insert(role);
            return IdentityResult.Success;
        }

        public async Task<MRRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
            => await Get(roleId);

        public async Task<MRRole> FindByNameAsync(string roleName, CancellationToken cancellationToken)
            => await Get(x => x.NormalizedName, roleName.ToUpperInvariant());

        public async Task<string> GetRoleIdAsync(MRRole role, CancellationToken cancellationToken)
            => (await GetByQueryFirst(_builder.Eq(x => x.NormalizedName, role.Name.ToUpperInvariant())))?.Id;

        public async Task<string> GetRoleNameAsync(MRRole role, CancellationToken cancellationToken)
            => (await GetByQueryFirst(_builder.Eq(x => x.Id, role.Id)))?.Name;

        public async Task<string> GetNormalizedRoleNameAsync(MRRole role, CancellationToken cancellationToken)
            => (await GetByQueryFirst(_builder.Eq(x => x.Id, role.Id)))?.NormalizedName;

        public async Task<IdentityResult> UpdateAsync(MRRole role, CancellationToken cancellationToken)
        {
            var exists = await Any(x => x.NormalizedName == role.Name.ToUpperInvariant());
            if (exists)
            {
                return IdentityResult.Failed(new IdentityError[] { new IdentityError
                {
                    Code = "0",
                    Description = "Role with this name already exists"
                } });
            }

            await UpdateByQuery(_builder.Eq(x => x.Id, role.Id).UpdateSet(x => x.Name, role.Name).UpdateSet(x => x.UpdateTime, DateTime.UtcNow).UpdateSet(x => x.NormalizedName, role.Name.ToUpperInvariant()));
            return IdentityResult.Success;
        }

        public async Task SetRoleNameAsync(MRRole role, string roleName, CancellationToken cancellationToken)
        {
            await UpdateManyByQuery(_builder.Eq(x => x.Id, role.Id).UpdateSet(x => x.Name, roleName).UpdateSet(x => x.NormalizedName, roleName.ToUpperInvariant()).UpdateSet(x => x.UpdateTime, DateTime.UtcNow));
        }

        public async Task SetNormalizedRoleNameAsync(MRRole role, string normalizedName, CancellationToken cancellationToken)
        {
            await UpdateManyByQuery(_builder.Eq(x => x.Id, role.Id).UpdateSet(x => x.NormalizedName, normalizedName).UpdateSet(x => x.UpdateTime, DateTime.UtcNow));
        }

        public async Task<IdentityResult> DeleteAsync(MRRole role, CancellationToken cancellationToken)
        {
            var exists = await Any(x => x.NormalizedName == role.Name.ToUpperInvariant());
            if (!exists)
            {
                return IdentityResult.Failed(new IdentityError[] { new IdentityError
                {
                    Code = "0",
                    Description = "Role not found"
                } });
            }

            await DeleteHard(role);
            return IdentityResult.Success;
        }

        public void Dispose() { }
    }
}
