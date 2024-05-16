using System.Threading.Tasks;
using OrchardCore.Data.Migration;
using OrchardCore.Layers.Indexes;
using OrchardCore.Layers.Services;
using OrchardCore.Rules.Services;
using YesSql.Sql;

namespace OrchardCore.Layers
{
    public class Migrations : DataMigration
    {
        private readonly ILayerService _layerService;
        private readonly IConditionIdGenerator _conditionIdGenerator;

        public Migrations(
            ILayerService layerService,
            IConditionIdGenerator conditionIdGenerator)
        {
            _layerService = layerService;
            _conditionIdGenerator = conditionIdGenerator;
        }

        public async Task<int> CreateAsync()
        {
            await SchemaBuilder.CreateMapIndexTableAsync<LayerMetadataIndex>(table => table
               .Column<string>("Zone", c => c.WithLength(64))
            );

            await SchemaBuilder.AlterIndexTableAsync<LayerMetadataIndex>(table => table
                .CreateIndex("IDX_LayerMetadataIndex_DocumentId",
                "DocumentId",
                "Zone")
            );

            // Shortcut other migration steps on new content definition schemas.
            return 3;
        }

        // This code can be removed in a later version.
        public async Task<int> UpdateFrom1Async()
        {
            await SchemaBuilder.AlterIndexTableAsync<LayerMetadataIndex>(table => table
                .CreateIndex("IDX_LayerMetadataIndex_DocumentId",
                "DocumentId",
                "Zone")
            );

            // Migration was cleaned up in version 2.0.
            // Jump to step 3 during create.
            return 3;
        }
    }
}
