using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLibrary.Core.Configurations.Base;
using MyLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Core.Configurations.Mapping
{
    internal class AuditMapping : AuditMappingBase
    {
        public override void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Action).HasColumnType("varchar(20)").IsRequired().HasMaxLength(20);
            builder.Property(e => e.TableName).HasColumnType("varchar(256)").IsRequired();
            builder.Property(e => e.Username).HasColumnType("varchar(256)").IsRequired();
            builder.ToTable("Audits");
            base.Configure(builder);
        }
    }
}
