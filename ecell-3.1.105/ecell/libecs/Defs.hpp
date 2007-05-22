//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//        This file is part of E-Cell Simulation Environment package
//
//                Copyright (C) 1996-2002 Keio University
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
// 
// E-Cell is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public
// License along with E-Cell -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
// 
//END_HEADER
//
// written by Koichi Takahashi <shafi@e-cell.org>,
// E-Cell Project.
//


#ifndef __DEFS_HPP
#define __DEFS_HPP
#include "ecell_config.h"

#include <float.h>
#include <string>
#include <list>
#include <vector>
#include <map>
#include <time.h>

#include <boost/call_traits.hpp>
#include <boost/smart_ptr.hpp>

#define DEBUG 1

// stringifiers.  see preprocessor manual
#define XSTR( S ) STR( S )
#define STR( S ) #S


#define USE_LIBECS using namespace libecs

// cmath

#if defined( HAVE_CMATH )
#include <cmath>
#elif defined( HAVE_MATH )
#include <math>
#else
#error "either math or cmath header is needed."
#endif /* HAVE_CMATH */
#if defined( USE_MSVC )
#define _USE_MATH_DEFINES
#include <math.h>
#undef _USE_MATH_DEFINES
#endif /* M_PI, M_E */


#if defined( HAVE_STDINT_H )
#include <stdint.h>
#endif /* HAVE_STDINT_H */

#if defined( HAVE_DLFCN_H )
#include <dlfcn.h>
#include <ltdl.h>
#endif /* HAVE_DLFCN_H */

#if defined( HAVE_WINDOWS_H )
#include <windows.h>
#if defined GetClassInfo
#undef GetClassInfo
#endif
#endif /* HAVE_WINDOWS_H */

// 
// If USE_COMPILER_EXTENSIONS is defined, the compiler's special
// language syntax and optimizations that are not part of the standard
// (such as ISO C++) are exploited.
//
// Defined macros:
//
// LIBECS_USE_PMF_CONVERSIONS 
// If this macro is defined, conversions from pointer-to-member-functions 
// to usual function pointers can be used.
//
//
// LIBECS_LIKELY( EXP ), LIBECS_UNLIKELY( EXP )
// These macros indicate the expression EXP is very (un)likely to be true,
// and the branch based on this will be frequently (not) taken.
// These are typically used in if() statements.   Unless you are very sure,
// it is a good idea to not to try to do this job by yourself and just 
// rely on the compiler and CPU's branch prediction mechanisms and 
// profile-based branch counters. These macros do nothing when 
// libecs does not support branch prediction on the platform.
//
//
// LIBECS_PREFETCH( ADDR, RW, LOCALITY )
// This macro prefetches the content of memory at the address ADDR,
// and refreshes the cache.   If RW is zero, the cache is prepared for
// a read access, and one for a write access.  LOCALITY (0..3) indicates
// the temporal locality of the access.   Larger values let the
// accessed addresses more sticky on the cache.
// These macros do nothing when libecs does not support prefetching
// on the platform.
//

#if defined( USE_COMPILER_EXTENSIONS ) && defined( __GNUC__ )
#    define LIBECS_USE_PMF_CONVERSIONS 1
#    define LIBECS_LIKELY( EXP )       __builtin_expect( ( EXP ), 1 )
#    define LIBECS_UNLIKELY( EXP )     __builtin_expect( ( EXP ), 0 )
#    define LIBECS_PREFETCH( ADDR, RW, LOCALITY )\
            __builtin_prefetch( ( ADDR ), ( RW ), ( LOCALITY ) )
#else
// do not define LIBECS_USE_PMF_CONVERSIONS
#    define LIBECS_LIKELY( EXP )       ( EXP )
#    define LIBECS_UNLIKELY( EXP )     ( EXP )
#    define LIBECS_PREFETCH            
#endif /* defined( USE_COMPILER_EXTENSIONS ) && defined( __GNUC__ ) */



namespace libecs
{

  // Some macros those origins are libCoreLinux++

  /**
     IGNORE_RETURN is an indicator that the return
     value for a function is ignored.
     i.e   IGNORE_RETURN getSomething( ... );
     Eliminates a lint warning.
  */

#define IGNORE_RETURN (void)

  /**
     Declare a new type and its pointer,
     const pointer, reference, and const reference types. For example
     DECLARE_TYPE( Dword, VeryLongTime );
     @param mydecl The base type
     @param mytype The new type
  */

#define DECLARE_TYPE( mydecl, mytype )  \
typedef mydecl         mytype;         \
typedef mytype *       mytype ## Ptr;  \
typedef const mytype * mytype ## Cptr; \
typedef mytype &       mytype ## Ref;  \
typedef const mytype & mytype ## Cref;

  /**
     Declare class , class pointer , 
     const pointer, class reference 
     and const class reference types for classes. For example
     DECLARE_CLASS( Exception );
     @param tag The class being declared
  */

#define DECLARE_CLASS( tag )            \
   class   tag;                        \
   typedef tag *       tag ## Ptr;     \
   typedef const tag * tag ## Cptr;    \
   typedef tag &       tag ## Ref;     \
   typedef const tag & tag ## Cref;

#define DECLARE_SHAREDPTR( type )\
  typedef boost::shared_ptr<type> type ## SharedPtr;\
  typedef type ## SharedPtr *       type ## SharedPtr ## Ptr;     \
  typedef const type ## SharedPtr * type ## SharedPtr ## Cptr;    \
  typedef type ## SharedPtr &       type ## SharedPtr ## Ref;     \
  typedef const type ## SharedPtr & type ## SharedPtr ## Cref;



  // *******************************************
  // Define the void pointer type.
  // *******************************************
   
  typedef void * VoidPtr;

  // *******************************************
  // Define the NULLPTR
  // *******************************************

#define  NULLPTR  0
   
  /**
     STL list template. This macro generates all
     the type references and pointers for the collection and
     respective iterators for a list.
     @param name The name you want to give the collection
     @param type The type object the collection manages
  */
#define DECLARE_LIST( type, name )                            \
      DECLARE_TYPE(std::list<type>,name);                       \
      typedef name::iterator name ## Iterator;                  \
      typedef name::iterator& name ## IteratorRef;              \
      typedef name::iterator* name ## IteratorPtr;              \
      typedef name::const_iterator name ## ConstIterator;       \
      typedef name::const_iterator& name ## ConstIteratorRef;   \
      typedef name::const_iterator* name ## ConstIteratorPtr;   \
      typedef name::reverse_iterator name ## Riterator;         \
      typedef name::reverse_iterator& name ## RiteratorRef;     \
      typedef name::reverse_iterator* name ## RiteratorPtr


  /**
     STL vector template. This macro generates all
     the type references and pointers for the collection and
     respective iterators for a vector.
     @param name The name you want to give the collection
     @param type The type for the vector
  */
#define DECLARE_VECTOR( type, name )                            \
   DECLARE_TYPE(std::vector<type>,name);                       \
   typedef name::iterator name ## Iterator;                    \
   typedef name::iterator& name ## IteratorRef;                \
   typedef name::iterator* name ## IteratorPtr;                \
   typedef name::const_iterator name ## ConstIterator;         \
   typedef name::const_iterator& name ## ConstIteratorRef;     \
   typedef name::const_iterator* name ## ConstIteratorPtr;     \
   typedef name::reverse_iterator name ## Riterator;           \
   typedef name::reverse_iterator& name ## RiteratorRef;       \
   typedef name::reverse_iterator* name ## RiteratorPtr


  /**
     STL set template. This macro generates all
     the type references and pointers for the collection and
     respective iterators for a set.
     @param name The name you want to give the collection
     @param key The object that represents the set key
     @param comp The comparator functor
  */
#define DECLARE_SET(key,comp,name)                                       \
      typedef set<key, comp > name;                                           \
      typedef name *       name ## Ptr;                                       \
      typedef const name * name ## Cptr;                                      \
      typedef name &       name ## Ref;                                       \
      typedef const name & name ## Cref;                                      \
      typedef name::iterator name ## Iterator;                                \
      typedef name::iterator& name ## IteratorRef;                            \
      typedef name::iterator* name ## IteratorPtr;                            \
      typedef name::const_iterator name ## ConstIterator;                     \
      typedef name::const_iterator& name ## ConstIteratorRef;                 \
      typedef name::const_iterator* name ## ConstIteratorPtr;                 \
      typedef name::reverse_iterator name ## Riterator;                       \
      typedef name::reverse_iterator& name ## RiteratorRef;                   \
      typedef name::reverse_iterator* name ## RiteratorPtr
   
  /**
     STL multiset template. This macro generates all
     the type references and pointers for the collection and
     respective iterators for a multiset.
     @param name The name you want to give the collection
     @param key The object that represents the mutliset key
     @param comp The comparator functor
  */
#define DECLARE_MULTISET(key,comp,name)                                  \
      typedef multiset<key, comp > name;                                      \
      typedef name *       name ## Ptr;                                       \
      typedef const name * name ## Cptr;                                      \
      typedef name &       name ## Ref;                                       \
      typedef const name & name ## Cref;                                      \
      typedef name::iterator name ## Iterator;                                \
      typedef name::iterator& name ## IteratorRef;                            \
      typedef name::iterator* name ## IteratorPtr;                            \
      typedef name::const_iterator name ## ConstIterator;                     \
      typedef name::const_iterator& name ## ConstIteratorRef;                 \
      typedef name::const_iterator* name ## ConstIteratorPtr;                 \
      typedef name::reverse_iterator name ## Riterator;                       \
      typedef name::reverse_iterator& name ## RiteratorRef;                   \
      typedef name::reverse_iterator* name ## RiteratorPtr


  /**
     STL map template. This macro generates all
     the type references and pointers for the collection and
     respective iterators for a map.
     @param name The name you want to give the collection
     @param key The object that represents the map key
     @param value The object that the key is associated to
     @param comp The comparator functor
  */
#define DECLARE_MAP(key,value,comp,name)                             \
      typedef std::map<key,value,comp > name;                      \
      typedef name *       name ## Ptr;                            \
      typedef const name * name ## Cptr;                           \
      typedef name &       name ## Ref;                            \
      typedef const name & name ## Cref;                           \
      typedef name::iterator name ## Iterator;                     \
      typedef name::iterator& name ## IteratorRef;                 \
      typedef name::iterator* name ## IteratorPtr;                 \
      typedef name::const_iterator name ## ConstIterator;          \
      typedef name::const_iterator& name ## ConstIteratorRef;      \
      typedef name::const_iterator* name ## ConstIteratorPtr;      \
      typedef name::reverse_iterator name ## Riterator;            \
      typedef name::reverse_iterator& name ## RiteratorRef;        \
      typedef name::reverse_iterator* name ## RiteratorPtr
   
  /**
     STL multimap template. This macro generates all
     the type references and pointers for the collection and
     respective iterators for a multimap.
     @param name The name you want to give the collection
     @param key The object that represents the map key
     @param value The object that the key is associated to
     @param comp The comparator functor
  */

#define DECLARE_MULTIMAP(key,value,comp,name)                 \
      typedef std::multimap<key,value,comp > name;                 \
      typedef name *       name ## Ptr;                            \
      typedef const name * name ## Cptr;                           \
      typedef name &       name ## Ref;                            \
      typedef const name & name ## Cref;                           \
      typedef name::iterator name ## Iterator;                     \
      typedef name::iterator& name ## IteratorRef;                 \
      typedef name::iterator* name ## IteratorPtr;                 \
      typedef name::const_iterator name ## ConstIterator;          \
      typedef name::const_iterator& name ## ConstIteratorRef;      \
      typedef name::const_iterator* name ## ConstIteratorPtr;      \
      typedef name::reverse_iterator name ## Riterator;            \
      typedef name::reverse_iterator& name ## RiteratorRef;        \
      typedef name::reverse_iterator* name ## RiteratorPtr


  /**
     STL queue template. This macro generates all
     the type references and pointers for the collection and
     respective iterators for a queue.
     @param name The name you want to give the collection
     @param type The type to be queued
  */
#define DECLARE_QUEUE( type, name )                          \
      DECLARE_TYPE(std::deque<type>,name);                     \
      typedef name::iterator name ## Iterator;                 \
      typedef name::iterator& name ## IteratorRef;             \
      typedef name::iterator* name ## IteratorPtr;             \
      typedef name::const_iterator name ## ConstIterator;      \
      typedef name::const_iterator& name ## ConstIteratorRef;  \
      typedef name::const_iterator* name ## ConstIteratorPtr;  \
      typedef name::reverse_iterator name ## Riterator;        \
      typedef name::reverse_iterator& name ## RiteratorRef;    \
      typedef name::reverse_iterator* name ## RiteratorPtr

  /**
     STL stack template. This macro generates all
     the type references and pointers for the collection and
     respective iterators for a stack.
     @param name The name you want to give the collection
     @param type The type to be stacked
  */
#define DECLARE_STACK( type, name )                                 \
      DECLARE_TYPE(stack<type>,name)                                   


  // from Loki

  
#define DECLARE_ASSOCVECTOR(key,value,comp,name)                             \
      typedef ::Loki::AssocVector<key,value,comp > name;                      \
      typedef name *       name ## Ptr;                            \
      typedef const name * name ## Cptr;                           \
      typedef name &       name ## Ref;                            \
      typedef const name & name ## Cref;                           \
      typedef name::iterator name ## Iterator;                     \
      typedef name::iterator& name ## IteratorRef;                 \
      typedef name::iterator* name ## IteratorPtr;                 \
      typedef name::const_iterator name ## ConstIterator;          \
      typedef name::const_iterator& name ## ConstIteratorRef;      \
      typedef name::const_iterator* name ## ConstIteratorPtr;      \
      typedef name::reverse_iterator name ## Riterator;            \
      typedef name::reverse_iterator& name ## RiteratorRef;        \
      typedef name::reverse_iterator* name ## RiteratorPtr

#define DECLARE_ASSOCVECTOR_TEMPLATE(key,value,comp,name)                \
      typedef ::Loki::AssocVector<key,value,comp > name;                      \
      typedef name *       name ## Ptr;                            \
      typedef const name * name ## Cptr;                           \
      typedef name &       name ## Ref;                            \
      typedef const name & name ## Cref;                           \
      typedef typename name::iterator name ## Iterator;                     \
      typedef typename name::iterator& name ## IteratorRef;                 \
      typedef typename name::iterator* name ## IteratorPtr;                 \
      typedef typename name::const_iterator name ## ConstIterator;          \
      typedef typename name::const_iterator& name ## ConstIteratorRef;      \
      typedef typename name::const_iterator* name ## ConstIteratorPtr;      \
      typedef typename name::reverse_iterator name ## Riterator;            \
      typedef typename name::reverse_iterator& name ## RiteratorRef;        \
      typedef typename name::reverse_iterator* name ## RiteratorPtr


  // Types

  template <typename T>
  class Param
  {
  public:
    typedef typename boost::call_traits<T>::param_type type;
  };

  // String

  DECLARE_TYPE( std::string, String );

  DECLARE_TYPE( const char* const, StringLiteral );

  // Numeric types

  DECLARE_TYPE( long int, Integer );
  DECLARE_TYPE( unsigned long int, UnsignedInteger );
  typedef Param<Integer>::type IntegerParam;
  typedef Param<UnsignedInteger>::type UnsignedIntegerParam;


  // these can cause problem when used as template parameters
  //  DECLARE_TYPE( int64_t, Integer );
  //  DECLARE_TYPE( uint64_t, UnsignedInteger );

  //  DECLARE_TYPE( double, Real );
  DECLARE_TYPE( double, Real );
  typedef Param<Real>::type RealParam;

#if defined( HAVE_LONG_DOUBLE )
  DECLARE_TYPE( long double, HighReal );
#else
  DECLARE_TYPE( double, HighReal );
  #define HIGHREAL_IS_REAL 1
#endif /* defined( HAVE_LONG_DOUBLE ) */
  typedef Param<HighReal>::type HighRealParam;
    
  //  DECLARE_TYPE( HighReal, Time );
  DECLARE_TYPE( Real, Time );
  typedef Param<Time>::type TimeParam;
    
  //! Infinity.  Currently this is defined as INFINITY symbol of C99 standard.
#if defined( HAVE_INFINITY )
  const Real INF( INFINITY );
#else
  const Real INF( HUGE_VAL );
#endif


  //! Avogadro number. 
  const Real N_A( 6.0221367e+23 );

  //! 1 / Avogadro number (reciprocal of N_A)
  const Real N_A_R( 1.0 / N_A );

  // functions

#if defined( FP_FAST_FMA )
  inline const Real FMA( const Real a, const Real b, const Real c )
  {
    return ::fma( a, b, c );
  }
#else
  inline const Real FMA( const Real a, const Real b, const Real c )
  {
    return a * b + c;
  }
#endif /* defined( FP_FAST_FMA ) */


  // MACROS

#if !defined( HAVE_PRETTY_FUNCTION )
#define __PRETTY_FUNCTION__ ""
#endif

  /**
     Converts each type into a unique, insipid type.
     Invocation Type2Type<T> where T is a type.
     Defines the type OriginalType which maps back to T.
     
     taken from loki library.

     @ingroup util
  */

  template <typename T>
  struct Type2Type
  {
    typedef T OriginalType;
  };


} // namespace libecs


/**
   4 MSVC
 */
/**
   DLL
 */
#if defined( USE_MSVC )
#if defined( ECELL_EXPORTS )
#define ECELL_API __declspec( dllexport )
#else
#define ECELL_API __declspec( dllimport )
#endif /* ECELL_EXPORTS */
#else
#define ECELL_API
#endif /* USE_MSVC */
/**
   Virtual Libtool
 */
#if defined( USE_MSVC )

#define DM_PATH "dm"
#define FILE_EXTENSION ".dll"
#define REGISTORY_DM_KEY L"E-CELL IDE DM"
#define REGISTORY_ENV_KEY L"Environment"
#define REGISTORY_SW_KEY L"Software\\KeioUniv\\E-Cell IDE"
#define SIZE 10000
class Util
{
private:
    static std::string getDMPath(HKEY aHKEY, LPCWSTR aSubKey)
    {
        HKEY hKey;
        DWORD dwType;
        DWORD dwSize;
        LONG errorCode = RegOpenKeyEx(aHKEY, aSubKey, 0, KEY_READ, &hKey);
        if(errorCode != 0)
        {
            return "";
        }
        wchar_t aValueWide[SIZE];
        errorCode = RegQueryValueEx(hKey, REGISTORY_DM_KEY, NULL, &dwType, NULL, &dwSize);
        if(errorCode != 0)
        {
            return "";
        }
        errorCode = RegQueryValueEx(hKey, REGISTORY_DM_KEY, NULL, &dwType, (LPBYTE)&aValueWide, &dwSize);
        if(errorCode != 0)
        {
            return "";
        }
        int buffSize = WideCharToMultiByte(CP_ACP, 0, aValueWide, -1, NULL, 0, NULL, NULL);
        if(buffSize <= 0)
        {
            return "";
        }
        LPSTR aValue = new char [buffSize];
        buffSize = WideCharToMultiByte(CP_ACP, 0, aValueWide, -1, aValue, buffSize, NULL, NULL);
        if(buffSize <= 0)
        {
            return "";
        }
        RegCloseKey(hKey);
        return aValue;
    }
public:
    static std::string getDMPath()
    {
        std::string aPath;
        if((aPath = Util::getDMPath(HKEY_CURRENT_USER, REGISTORY_ENV_KEY)).length() > 0)
        {
            return aPath;
        }
        else if((aPath = Util::getDMPath(HKEY_CURRENT_USER, REGISTORY_SW_KEY)).length() > 0)
        {
            return aPath;
        }
        else
        {
            return Util::getDMPath(HKEY_LOCAL_MACHINE, REGISTORY_SW_KEY);
        }
    }
};
#define lt_dlclose(x) FreeLibrary(x)
#define lt_dlhandle HMODULE
#define lt_dlsym(x, y) GetProcAddress(x, y)
static char errbuf[64];
static char * lt_dlerror()
{
  sprintf_s(errbuf, "LoadLibrary/GetProcAddress error %d", static_cast<int>(GetLastError()));
  return errbuf;
}
static int lt_dlexit()
{
    return 0;
}
static const char * lt_dlgetsearchpath()
{
    return 0;
}
static int lt_dlinit()
{
    return 0;
}
static lt_dlhandle lt_dlopenext(const char* aFileName)
{
    if (aFileName == NULL)
    {
        return NULL;
    }
    std::string anAllDMPath = Util::getDMPath();
    if (anAllDMPath.size() < 1)
    {
      anAllDMPath = DM_PATH;
    }
    while (anAllDMPath.size() > 0)
    {
        int anIndex = static_cast<int>(anAllDMPath.find(";"));
        char aDMPath[SIZE];
        if(anIndex == -1)
        {
            strcpy_s(aDMPath, anAllDMPath.c_str());
            anAllDMPath.clear();
        }
        else if(anIndex == anAllDMPath.size() - 1)
        {
            strcpy_s(aDMPath, anAllDMPath.substr(0, anIndex).c_str());
            anAllDMPath.clear();
        }
        else
        {
            strcpy_s(aDMPath, anAllDMPath.substr(0, anIndex).c_str());
            anAllDMPath = anAllDMPath.substr(anIndex + 1);
        }
        std::string dllNameStr = std::string(aDMPath) + "/" + aFileName + FILE_EXTENSION;
        char dllName[SIZE];
        strcpy_s(dllName, dllNameStr.c_str());
        wchar_t dllNameWide[SIZE];
        MultiByteToWideChar(CP_ACP, 0, dllName, -1, dllNameWide, SIZE);
        lt_dlhandle aHandle = LoadLibrary(dllNameWide);
        if (aHandle != NULL)
        {
            return aHandle;
        }
    }
    return NULL;
}
static int lt_dlsetsearchpath(const char *search_path)
{
    return 0;
}
/**
   Some Functions
 */
#define fmin(x, y) __min(x, y)
#endif /* USE_MSVC */

#if defined( USE_MSVC )
#define staticFlag false
#else
#define staticFlag true
#endif

#endif /* __DEFS_HPP */

/*
  Do not modify
  $Author: shafi $
  $Revision: 2559 $
  $Date: 2006-05-27 19:28:17 -0700 (Sat, 27 May 2006) $
  $Locker$
*/
