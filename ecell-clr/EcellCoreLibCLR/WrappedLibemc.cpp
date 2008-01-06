#include "WrappedLibemc.hpp"

using namespace System;


namespace EcellCoreLib {
    WrappedEventChecker::WrappedEventChecker( boost::shared_ptr< libemc::EventChecker > * aEventChecker ) {
        WrappedEventChecker::theEventChecker = aEventChecker;
    }

    boost::shared_ptr< libemc::EventChecker > * WrappedEventChecker::getLibemcEventChecker() {
        return WrappedEventChecker::theEventChecker;
    }

    WrappedEventHandler::WrappedEventHandler( boost::shared_ptr< libemc::EventHandler > * aEventHandler ) {
        WrappedEventHandler::theEventHandler = aEventHandler;
    }

    boost::shared_ptr< libemc::EventHandler > * WrappedEventHandler::getLibemcEventHandler() {
        return WrappedEventHandler::theEventHandler;
    }

}
