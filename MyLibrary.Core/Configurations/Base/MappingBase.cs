using MyLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Core.Configurations.Base
{
    public abstract class AuditMappingBase : BaseEntityTypeConfiguration<Audit> { }
    public abstract class RefreshTokenMappingBase : BaseEntityTypeConfiguration<RefreshToken> { }
}
