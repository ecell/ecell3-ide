//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//       This file is part of the E-Cell System
//
//       Copyright (C) 1996-2010 Keio University
//       Copyright (C) 2005-2008 The Molecular Sciences Institute
//
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
//
//
// E-Cell System is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
// 
// E-Cell System is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public
// License along with E-Cell System -- see the file COPYING.
// If not, write to the Free Software Foundation, Inc.,
// 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
// 
//END_HEADER

#include <libecs/Process.hpp>
#include <libecs/Exceptions.hpp>
#include <vector>

USE_LIBECS;

LIBECS_DM_CLASS( MassCalculationProcess, Process )
{
private:

	DECLARE_VECTOR( Real, RealVector );

public:

	LIBECS_DM_OBJECT( MassCalculationProcess, Process )
	{
		INHERIT_PROPERTIES( Process );
	}

	virtual void initialize()
	{
		Process::initialize();
		theMassVector.resize( theVariableReferenceVector.size() );

		RealVector::iterator j( theMassVector.begin() );
		for ( VariableReferenceVector::const_iterator
			i( theVariableReferenceVector.begin() ),
			e( theVariableReferenceVector.end() );
			i != e; ++i )
		{
			try
			{
				*j = stringCast< Real >( ( *i ).getName() );
				++j;
			}
			catch ( std::bad_cast const& e )
			{
				THROW_EXCEPTION_INSIDE( ValueError, "Expected the molar mass for ["
									    + ( *i ).getVariable()->asString() + "], "
										"got " + ( *i ).getName() );
			}
		}
	}

	virtual void fire()
	{
		Real aTotal( 0. );
		RealVector::const_iterator j( theMassVector.begin() );
		for ( VariableReferenceVector::const_iterator
			i( theVariableReferenceVector.begin() ),
			e( theVariableReferenceVector.end() );
			i != e; ++i, ++j )
		{
			aTotal += *j * ( *i ).getVariable()->getValue();
		}
		setActivity( aTotal );
	}

private:

	RealVector theMassVector;
};

LIBECS_DM_INIT( MassCalculationProcess, Process );