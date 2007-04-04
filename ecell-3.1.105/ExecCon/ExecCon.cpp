#include <tchar.h>
#include <time.h>
#include <iostream>
#include "libemc/Simulator.hpp"
#include "libecs/VVector.h"

using namespace libecs;

int _tmain(int argc, _TCHAR* argv[])
{
    int l_sepa = 0;
    //
    //
    //
    libemc::Simulator l_simulator;
    std::vector<libecs::Polymorph> l_polymorphVector;
    std::vector<libecs::Polymorph> l_childPolymorphVector0;
    std::vector<libecs::Polymorph> l_childPolymorphVector1;
    std::vector<libecs::Polymorph> l_childPolymorphVector2;
    std::vector<libecs::Polymorph> l_childPolymorphVector3;
    std::vector<libecs::Polymorph> l_childPolymorphVector4;
    std::vector<libecs::Polymorph> l_descendantPolymorphVector0;
    std::vector<libecs::Polymorph> l_descendantPolymorphVector1;
    std::vector<libecs::Polymorph> l_descendantPolymorphVector2;
    std::vector<std::string> l_loggerVector;
    std::vector<libecs::Polymorph> l_loggerPolicy;
    FILE * fout;
    //
    // Test of GetProcessProperty
    //
    l_sepa = 0;
/*
    l_simulator.createStepper("FixedODE1Stepper", "DefaultStepper");
    l_simulator.createEntity("Variable", "Variable:/:SIZE");
    // l_simulator.createEntity("ExpressionFluxProcess", "Process:/:SIZE");
    l_simulator.createEntity("MichaelisUniUniFluxProcess", "Process:/:SIZE");
    l_polymorphVector.clear();
    l_polymorphVector.push_back(libecs::Polymorph("DefaultStepper"));
    l_simulator.loadEntityProperty("System::/:StepperID", l_polymorphVector);
    l_polymorphVector.clear();
    l_polymorphVector.push_back(libecs::Polymorph("0.1"));
    l_simulator.loadEntityProperty("Variable:/:SIZE:Value", l_polymorphVector);
    l_polymorphVector.clear();
    l_childPolymorphVector0.clear();
    l_childPolymorphVector0.push_back(libecs::Polymorph("S0"));
    l_childPolymorphVector0.push_back(libecs::Polymorph(":.:SIZE"));
    l_childPolymorphVector0.push_back(libecs::Polymorph("0"));
    l_polymorphVector.push_back(libecs::Polymorph(l_childPolymorphVector0));
    l_simulator.loadEntityProperty("Process:/:SIZE:VariableReferenceList", l_polymorphVector);
    libecs::Polymorph pol = l_simulator.getEntityProperty("Process:/:SIZE:Activity");
    printf("%d\n", pol.asReal());
*/
    l_sepa = 0;
    //
    // sample/simple ÇÃâºëzé¿çs
    // 
    l_sepa = 0;
    l_simulator.createStepper("FixedODE1Stepper", "DE1");
    l_simulator.createEntity("Variable", "Variable:/:SIZE");
    l_simulator.createEntity("Variable", "Variable:/:S");
    l_simulator.createEntity("Variable", "Variable:/:P");
    l_simulator.createEntity("Variable", "Variable:/:E");
    l_simulator.createEntity("MichaelisUniUniFluxProcess", "Process:/:E");
    //
    l_polymorphVector.clear();
    l_polymorphVector.push_back(libecs::Polymorph("DE1"));
    l_simulator.loadEntityProperty("System::/:StepperID", l_polymorphVector);
    l_polymorphVector.clear();
    l_polymorphVector.push_back(libecs::Polymorph("1e-18"));
    l_simulator.loadEntityProperty("Variable:/:SIZE:Value", l_polymorphVector);
    l_polymorphVector.clear();
    l_polymorphVector.push_back(libecs::Polymorph("1000000"));
    l_simulator.loadEntityProperty("Variable:/:S:Value", l_polymorphVector);
    l_polymorphVector.clear();
    l_polymorphVector.push_back(libecs::Polymorph("0"));
    l_simulator.loadEntityProperty("Variable:/:P:Value", l_polymorphVector);
    l_polymorphVector.clear();
    l_polymorphVector.push_back(libecs::Polymorph("1000"));
    l_simulator.loadEntityProperty("Variable:/:E:Value", l_polymorphVector);
    l_polymorphVector.clear();
    l_childPolymorphVector0.clear();
    l_childPolymorphVector1.clear();
    l_childPolymorphVector2.clear();
    l_childPolymorphVector0.push_back(libecs::Polymorph("S0"));
    l_childPolymorphVector0.push_back(libecs::Polymorph(":.:S"));
    l_childPolymorphVector0.push_back(libecs::Polymorph("-1"));
    l_childPolymorphVector1.push_back(libecs::Polymorph("P0"));
    l_childPolymorphVector1.push_back(libecs::Polymorph(":.:P"));
    l_childPolymorphVector1.push_back(libecs::Polymorph("1"));
    l_childPolymorphVector2.push_back(libecs::Polymorph("C0"));
    l_childPolymorphVector2.push_back(libecs::Polymorph(":.:E"));
    l_childPolymorphVector2.push_back(libecs::Polymorph("0"));
    l_polymorphVector.push_back(libecs::Polymorph(l_childPolymorphVector0));
    l_polymorphVector.push_back(libecs::Polymorph(l_childPolymorphVector1));
    l_polymorphVector.push_back(libecs::Polymorph(l_childPolymorphVector2));
    l_simulator.loadEntityProperty("Process:/:E:VariableReferenceList", l_polymorphVector);
    l_polymorphVector.clear();
    l_polymorphVector.push_back(libecs::Polymorph("1"));
    l_simulator.loadEntityProperty("Process:/:E:KmS", l_polymorphVector);
    l_polymorphVector.clear();
    l_polymorphVector.push_back(libecs::Polymorph("10"));
    l_simulator.loadEntityProperty("Process:/:E:KcF", l_polymorphVector);
    l_simulator.getEntityProperty("System::/:Name");
    l_simulator.setEntityProperty("Process:/:E:Activity", libecs::Polymorph("500000"));
    // l_simulator.initialize();
    //
    // l_loggerVector.push_back("Variable:/:SIZE:Value");
    // l_loggerVector.push_back("Variable:/:E:Value");
    // l_loggerVector.push_back("Variable:/:P:Value");
    // l_loggerVector.push_back("Variable:/:S:Value");
    // l_loggerVector.push_back("Variable:/:P:MolarConc");
    // l_loggerVector.push_back("Variable:/:P:NumberConc");
    // l_loggerVector.push_back("Variable:/:P:Value");
    // l_loggerVector.push_back("Variable:/:P:Velocity");
    l_loggerVector.push_back("Process:/:E:Activity");
    //
    fout = fopen("20070215_simple.log", "w");
    //
    l_sepa = 0;
    //
    // Logger
    //
    l_loggerPolicy.push_back(libecs::Polymorph(libecs::Integer(1)));
    l_loggerPolicy.push_back(libecs::Polymorph(0.0));
    l_loggerPolicy.push_back(libecs::Polymorph(libecs::Integer(0)));
    l_loggerPolicy.push_back(libecs::Polymorph(libecs::Integer(0)));
    for(unsigned int i = 0; i < l_loggerVector.size(); i++)
    {
        l_simulator.createLogger(l_loggerVector[i], l_loggerPolicy);
    }
    //
    // Value
    //
    for(unsigned int i = 0; i < l_loggerVector.size(); i++)
    {
        //printf("%lf\t\"%s\", \"%25.15lf\"\r\n", l_endTime, l_loggerVector[i].c_str(), l_value);
        /*
        fprintf(
                fout, "%lf\t%s, %25.15lf\n",
                0.0, l_loggerVector[i].c_str(), l_simulator.getEntityProperty(l_loggerVector[i]).asReal());
         */
        fprintf(
                fout, "%e\t%s, %25.15e\n",
                0.0, l_loggerVector[i].c_str(), l_simulator.getEntityProperty(l_loggerVector[i]).asReal());
    }
    //
    // Step
    //
	time_t present;
	struct tm *ltime;
	char * now;
    int j = 0;
    while (j < 1)
    {
        double l_startTime = l_simulator.getCurrentTime();
        // double l_startTime = 0.0;
        try
        {
            // l_simulator.step(1000);
            l_simulator.run(1000.0);
        }
        catch(vvector_full l_ex)
        {
            printf("Fatal Error: %s\r\n", l_ex.what());;
            exit(1);
        }
        catch(vvector_write_error l_ex)
        {
            printf("Fatal Error: %s\r\n", l_ex.what());;
            exit(2);
        }
        catch(vvector_read_error l_ex)
        {
            printf("Fatal Error: %s\r\n", l_ex.what());;
            exit(3);
        }
        catch(vvector_init_error l_ex)
        {
            printf("Fatal Error: %s\r\n", l_ex.what());;
            exit(4);
        }
        double l_endTime = l_simulator.getCurrentTime();
        for(unsigned int i = 0; i < l_loggerVector.size(); i++)
        {
        	time(&present);
	        ltime = localtime(&present);
	        now = asctime(ltime);
            libecs::DataPointVectorSharedPtr l_point =
            //     l_simulator.getLoggerData(l_loggerVector[i], l_startTime, l_endTime, 100.0);
                   l_simulator.getLoggerData(l_loggerVector[i], l_startTime, l_endTime, 100.0);
        	time(&present);
	        ltime = localtime(&present);
	        now = asctime(ltime);
            // printf("\t1; %s", now);
            // fprintf(fout, "\t1; %s", now);
            for(DataPointVectorIterator l_i = l_point -> begin();
                l_i < l_point -> end();
                l_i++
                )
            {
                DataPointRef l_dataPoint = l_point->asShort(l_i);
                fprintf(fout, "%15.15e\t%s, %25.15e\n",
                    l_dataPoint.getTime(), l_loggerVector[i].c_str(), l_dataPoint.getValue());
            }
            /*
            libecs::DataPointRef l_dataPoint = l_point -> asShort((l_point -> end()) - 1);
        	time(&present);
	        ltime = localtime(&present);
	        now = asctime(ltime);
            double l_value = l_dataPoint.getValue();
            fprintf(fout, "%15.15e\t%s, %25.15e\n", l_endTime, l_loggerVector[i].c_str(), l_value);
            */
            fflush(fout);
        }
        fflush(fout);
        j++;
    }
    fclose(fout);
    return 0;
}

