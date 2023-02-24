public class genlist<T> //T: Type paramater
{
	public T[] data;
	public int size => data.Length;
	public T this[int i] => data[i];
	public genlist()
	{
		data = new T[0];
	}
	public void add(T item)
	{
		T[] newdata = new T[size+1];
		System.Array.Copy(data,newdata,size); // copies size from data to newcopy
		newdata[size] = item;
		data = newdata;
	}
}
