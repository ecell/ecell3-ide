#include "libecs/Process.hpp"
#include "ExpressionCompiler.hpp"
#include "ExpressionProcessBase.hpp"

namespace libecs { // USE_LIBECS;

LIBECS_DM_CLASS(PythonProcess, ExpressionProcessBase)
{
public:

    LIBECS_DM_OBJECT(PythonProcess, Process)
    {
        INHERIT_PROPERTIES(ExpressionProcessBase);
        PROPERTYSLOT(String, Expression, NULLPTR, NULLPTR);
        PROPERTYSLOT_SET_GET(Integer, IsContinuous);
        PROPERTYSLOT_SET_GET(String, FireMethod);
        PROPERTYSLOT_SET_GET(String, InitializeMethod);
    }

    PythonProcess()
        :
        theIsContinuous(false)
    {
        setInitializeMethod( "" );
        setFireMethod( "" );
    }

    ~PythonProcess()
    {
        ; // do nothing
    }

    void compileExpression()
    {
        ExpressionCompiler theCompiler(this, &thePropertyMap);
        theCompiledCode.clear();
        String codedMethod = "";
        if (theFireMethod.find('=') != String::npos)
        {
            codedMethod = theFireMethod.substr(theFireMethod.find('=') + 1);
        }
        else if (theFireMethod.find('(') != String::npos)
        {
            codedMethod = theFireMethod.substr(theFireMethod.find('('));
        }
        else
        {
            codedMethod = theFireMethod;
        }
        trim(codedMethod);
        theCompiledCode = theCompiler.compileExpression(codedMethod);
    }

    virtual void fire()
    {
        Real value = theVirtualMachine.execute(theCompiledCode);
        if (theFireMethod.find('=') != String::npos)
        {
            String setMethodStr = theFireMethod.substr(0, theFireMethod.find('='));
            trim(setMethodStr);
            if (setMethodStr.find('.'))
            {
                String variableName = setMethodStr.substr(0, setMethodStr.find('.'));
                String methodName = setMethodStr.substr(setMethodStr.find('.') + 1);
                if (methodName == "Value")
                {
                    getVariableReference(variableName).setValue(value);
                }
                else
                {
                    THROW_EXCEPTION(NotFound, 
                        "PythonProcess: VariableReference attribute [" + methodName + "] not found.");
                }
            }
            else
            {
                THROW_EXCEPTION(NotFound, 
                    "PythonProcess: VariableReference attribute [" + setMethodStr + "] not found.");
            }
        }
        else if (theFireMethod.find('(') != String::npos)
        {
            String setMethodStr = theFireMethod.substr(0, theFireMethod.find('('));
            trim(setMethodStr);
            if (setMethodStr == "self.setFlux")
            {
                setFlux(value);
            }
            else
            {
                THROW_EXCEPTION(NotFound, 
                    "PythonProcess: VariableReference attribute [" + setMethodStr + "] not found.");
            }
        }
        else
        {
            THROW_EXCEPTION(NotFound, 
                "PythonProcess: VariableReference attribute [" + theFireMethod + "] not found.");
        }
    }

    GET_METHOD(String, FireMethod)
    {
        return theFireMethod;
    }

    virtual void initialize()
    { 
        Process::initialize();
        if (theRecompileFlag)
        {
            compileExpression();
            theRecompileFlag = false;
        }
    }

    GET_METHOD(String, InitializeMethod)
    {
        return theInitializeMethod;
    }

    virtual const bool isContinuous() const
    {
        return theIsContinuous;
    }

    SET_METHOD(String, FireMethod)
    {
        theFireMethod = value;
        theRecompileFlag = true;
        /*
        theCompiledFireMethod
                = compilePythonCode( theFireMethod, getFullID().getString() +
						  ":FireMethod",
						  Py_file_input );
        */
    }

    SET_METHOD(String, InitializeMethod)
    {
        theInitializeMethod = value;
        String initializeMethod = value;
        while (initializeMethod.length() > 0)
        {
            String segment = "";
            if(initializeMethod.find(';') != String::npos)
            {
                segment = initializeMethod.substr(0, initializeMethod.find(';'));
                initializeMethod = initializeMethod.substr(initializeMethod.find(';') + 1);
            }
            else
            {
                segment = initializeMethod;
                initializeMethod = "";
            }
            String key = segment.substr(0, segment.find('='));
            trim(key);
            String value = segment.substr(segment.find('=') + 1);
            trim(value);
            thePropertyMap[key] = convertTo<Real>(value);
        }
    }

    SET_METHOD(Integer, IsContinuous)
    {
        theIsContinuous = value;
    }

    void trim(String &value)
    {
        if (value.find(' ') == 0)
        {
            value = value.substr(1);
        }
        if (value.rfind(' ') == value.length() - 1)
        {
            value = value.substr(0, value.length() - 1);
        }
    }

protected:

    bool theIsContinuous;
    bool theRecompileFlag;
    ExpressionCompiler::Code theCompiledCode;
    String theFireMethod;
    String theInitializeMethod;
    PropertyMap thePropertyMap;
    ExpressionProcessBase::VirtualMachine theVirtualMachine;
};

LIBECS_DM_INIT(PythonProcess, Process);
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
