#pragma once

#include "libemc/libemc.hpp"

using namespace System;


namespace EcellCoreLib {
    public ref class WrappedEventChecker {
    private:
        boost::shared_ptr< libemc::EventChecker > * theEventChecker;
    public:
        WrappedEventChecker( boost::shared_ptr< libemc::EventChecker > * aEventChecker );
        boost::shared_ptr< libemc::EventChecker > * getLibemcEventChecker();
    };

    public ref class WrappedEventHandler {
    private:
        boost::shared_ptr< libemc::EventHandler > * theEventHandler;
    public:
        WrappedEventHandler( boost::shared_ptr< libemc::EventHandler > * aEventHandler );
        boost::shared_ptr< libemc::EventHandler > * getLibemcEventHandler();
    };
}
