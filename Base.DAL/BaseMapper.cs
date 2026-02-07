using Base.Mapper.Contracts;

namespace Base.DAL;

public class BaseMapper<TSource, TDestination>: IMapper<TSource, TDestination>
{
    protected readonly AutoMapper.IMapper Mapper;
    
    public BaseMapper(AutoMapper.IMapper mapper)
    {
        Mapper = mapper;
    }
    
    public virtual TSource? Map(TDestination? entity)
    {
        return Mapper.Map<TSource>(entity);
    }

    public virtual TDestination? Map(TSource? entity)
    {
        return Mapper.Map<TDestination>(entity);
    }
}