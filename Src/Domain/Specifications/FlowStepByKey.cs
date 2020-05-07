//using System;
//using System.Linq.Expressions;
//using Domain.Entities;
//using Domain.Interfaces;

//namespace Domain.Specifications
//{
//    public class FlowStepByKey : ISpecification<FlowStepEntity>
//{
//    public FlowStepByKey(string key)
//    {
//        Criteria = e => e.Key == key;
//    }

//    public Expression<Func<FlowStepEntity, bool>> Criteria { get; }
//}
//}