namespace ProjetoJqueryEstudos.Service
{
    public static class Mapper
    {

        public static TViewModel? MapToViewModel<TEntity, TViewModel>(TEntity entity)
            where TEntity : class
            where TViewModel : class, new()
        {
            if (entity == null)
                return null;

            var viewModel = new TViewModel();

            foreach (var entityProperty in typeof(TEntity).GetProperties())
            {
                var viewModelProperty = typeof(TViewModel).GetProperty(entityProperty.Name);

                if (viewModelProperty != null)
                {
                    viewModelProperty.SetValue(viewModel, entityProperty.GetValue(entity));
                }
            }

            return viewModel;
        }

    }
}
