using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL
{
    public class DbContext<T> where T : class, new()
    {
        public DbContext()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnOracleWithAddress"].ConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });
        }

        public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作

        public SimpleClient<T> CurrentDb { get { return new SimpleClient<T>(Db); } }//用来处理T表的常用操作

        #region 查询

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList(Expression<Func<T, bool>> whereExpression = null)
        {
            return whereExpression == null
                ? CurrentDb.GetList() : Db.Queryable<T>().Where(whereExpression).ToList();
        }

        /// <summary>
        /// 查询 延迟加载
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public virtual ISugarQueryable<T> GetSugarQueryable(Expression<Func<T, bool>> whereExpression = null)
        {
            return whereExpression == null ? Db.Queryable<T>() : Db.Queryable<T>().Where(whereExpression);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <returns></returns>
        public virtual T GetById(int Id)
        {
            return CurrentDb.GetById(Id);
        }

        #endregion

        #region 插入

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Insert(T obj)
        {
            return CurrentDb.Insert(obj);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool Insert(List<T> TList)
        {
            return CurrentDb.InsertRange(TList);
        }

        #endregion

        #region 删除

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(dynamic id)
        {
            return CurrentDb.DeleteById(id);
        }

        /// <summary>
        /// 根据条件(linq)删除数据
        /// </summary>
        /// <param name="whereExpression"></param>
        public virtual bool DeleteByWhere(Expression<Func<T, bool>> whereExpression)
        {
            return CurrentDb.Delete(whereExpression);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Delete(T t)
        {
            return CurrentDb.Delete(t);
        }

        /// <summary>
        /// 根据主键 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual bool DeleteList(string[] ids)
        {
            return CurrentDb.DeleteByIds(ids);
        }

        #endregion

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Update(T obj)
        {
            return CurrentDb.Update(obj);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool UpdateRange(List<T> TList)
        {
            return CurrentDb.UpdateRange(TList);
        }

        #endregion

        #region sqlsugar执行sql语句

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <returns></returns>
        public virtual ISugarQueryable<T> GetSqlQueryable(string sql)
        {
            return Db.SqlQueryable<T>(sql);
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetListRF(string sql)
        {
            return Db.SqlQueryable<T>(sql).ToList();
        }

        /// <summary>
        /// sql增删改
        /// </summary>
        /// <param name="sql"></param>
        public virtual void SqlAddDelUp(string sql)
        {
            Db.AddQueue(sql);
        }

        /// <summary>
        /// sql增删改 带参数
        /// </summary>
        /// <param name="sql"></param>
        public virtual void SqlAddDelUpByParam(string sql, SugarParameter param)
        {
            Db.AddQueue(sql, param);
        }

        #endregion
    }
}
